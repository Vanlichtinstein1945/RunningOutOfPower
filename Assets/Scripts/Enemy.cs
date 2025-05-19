using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Weapon equippedWeapon;
    private float orbitRadius = 1.5f;
    private Vector2 lastKnownAimDirection = Vector2.right;
    private Vector2 lastKnownTargetPosition;

    public float detectionRange = 11.5f;
    public LayerMask obstacleLayer;
    private Transform target;

    private bool targetDetected = false;
    private float timeSinceLastSeen = 0f;
    public float maxMemoryTime = 3f;

    public GameObject healthObject;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentHealth = maxHealth;
        equippedWeapon = GetComponentInChildren<Weapon>();
        equippedWeapon.isFriendlyOwned = false;
        target = GameObject.FindWithTag("Player")?.transform;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GameManager.instance.isPaused)
        {
            return;
        }

        UpdateHealth();

        if (target == null)
        {
            target = GameObject.FindWithTag("Player")?.transform;
        }

        bool canSeeTarget = HasLineOfSightToTarget();

        if (canSeeTarget)
        {
            targetDetected = true;
            timeSinceLastSeen = 0f;
            lastKnownTargetPosition = target.position;
        }
        else if (targetDetected)
        {
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen >= maxMemoryTime)
            {
                targetDetected = false;
                return;
            }
        }

        if (targetDetected)
        {
            HandleAiming();

            if (canSeeTarget)
            {
                equippedWeapon?.Shoot();
            }
        }
    }

    private bool HasLineOfSightToTarget()
    {
        if (target == null)
        {
            return false;
        }

        Vector2 origin = transform.position;
        Vector2 direction = (Vector2)target.position - origin;
        float distance = direction.magnitude;

        if (distance > detectionRange)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, distance, obstacleLayer);

        return hit.collider == null;
    }

    private void HandleAiming()
    {
        Vector2 dirToLastSeen = lastKnownTargetPosition - (Vector2)transform.position;

        if (dirToLastSeen.sqrMagnitude > 0.1f)
        {
            lastKnownAimDirection = dirToLastSeen.normalized;
        }

        Vector3 offset = (Vector3)(lastKnownAimDirection * orbitRadius);
        equippedWeapon.transform.position = transform.position + offset;

        float angle = Mathf.Atan2(lastKnownAimDirection.y, lastKnownAimDirection.x) * Mathf.Rad2Deg;
        equippedWeapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void UpdateHealth()
    {
        float healthPercent = Mathf.Clamp01((float)currentHealth / (float)maxHealth);
        Vector3 baseScale = new Vector3(0.9f, 0.9f, 1f);

        Vector3 scale = baseScale;
        scale.y = baseScale.y * healthPercent;
        healthObject.transform.localScale = scale;

        float fullHeight = baseScale.y;
        float newHeight = fullHeight * healthPercent;
        float yOffset = -(fullHeight - newHeight) * 0.5f;

        Vector3 position = healthObject.transform.localPosition;
        position.y = yOffset;
        healthObject.transform.localPosition = position;
    }
}