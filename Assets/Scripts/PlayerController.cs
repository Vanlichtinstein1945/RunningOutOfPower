using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction aimAction;
    public InputAction shootingAction;

    public float moveSpeed = 5f;
    public int maxHealth = 100;
    private int currentHealth;
    private Vector2 moveInput;

    private Rigidbody2D rb;

    private Transform weapon;
    private float orbitRadius = 1.5f;
    private Vector2 lastAimDirection = Vector2.right;
    private Vector2 lastMousePos;
    private enum AimSource { Controller, Mouse }
    private AimSource lastAimSource = AimSource.Mouse;
    private Vector2 aimInput;
    private float shootingInput;

    public Transform chargeSprite;

    private void OnEnable()
    {
        moveAction.Enable();
        aimAction.Enable();
        shootingAction.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        aimAction.Disable();
        shootingAction.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentHealth = maxHealth;
        GameObject weaponObj = GetComponentInChildren<Weapon>().gameObject;
        weapon = weaponObj.transform;
        weaponObj.GetComponent<Weapon>().isFriendlyOwned = true;
        rb = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (GameManager.instance.isPaused || GameManager.instance.playerIsDead)
        {

        }
        else
        {
            // Read controller input
            moveInput = moveAction.ReadValue<Vector2>();
            aimInput = aimAction.ReadValue<Vector2>();
            shootingInput = shootingAction.ReadValue<float>();
        }

        if (lastAimSource == AimSource.Mouse || GameManager.instance.isPaused || GameManager.instance.playerIsDead)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isPaused || GameManager.instance.playerIsDead)
        {

        }
        else
        {
            HandleMovement();
            HandleAiming();

            if (shootingInput > 0.5f)
            {
                weapon.GetComponent<Weapon>().Shoot();
            }

            UpdateChargeDisplay();
        }
    }

    private void HandleMovement()
    {
        // Move player
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.deltaTime);
    }

    private void HandleAiming()
    {
        if (MouseMovedThisFrame())
        {
            lastAimSource = AimSource.Mouse;
        }

        // Aim weapon
        if (aimInput.sqrMagnitude > 0.5f)
        {
            lastAimSource = AimSource.Controller;
            lastAimDirection = aimInput.normalized;
        }
        // If mouse is being used, then aim with mouse, otherwise use last controller input
        else if (lastAimSource == AimSource.Mouse)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 dirToMouse = mouseWorldPos - transform.position;

            if (dirToMouse.sqrMagnitude > 0.1f)
            {
                lastAimDirection = dirToMouse.normalized;
            }
        }

        Vector3 weaponOffset = (Vector3)(lastAimDirection * orbitRadius);
        weapon.position = transform.position + weaponOffset;

        float angle = Mathf.Atan2(lastAimDirection.y, lastAimDirection.x) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.Euler(0, 0, angle);
    }

    private bool MouseMovedThisFrame()
    {
        Vector2 currentPos = Mouse.current.position.ReadValue();
        bool moved = (currentPos - lastMousePos).sqrMagnitude > 0.5f;
        lastMousePos = currentPos;
        return moved;
    }

    private void UpdateChargeDisplay()
    {
        // Update scale and position of health sprite based on percentage
        float healthPercent = Mathf.Clamp01((float)currentHealth / (float)maxHealth);
        Vector3 baseScale = new Vector3(0.9f, 0.9f, 1f);

        Vector3 scale = baseScale;
        scale.y = baseScale.y * healthPercent;
        chargeSprite.localScale = scale;

        float fullHeight = baseScale.y;
        float newHeight = fullHeight * healthPercent;
        float yOffset = -(fullHeight - newHeight) * 0.5f;

        Vector3 position = chargeSprite.localPosition;
        position.y = yOffset;
        chargeSprite.localPosition = position;
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.instance.playerIsDead)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            GameManager.instance.ToggleDeathScreen();
        }
    }

    public void SwapWeapon(GameObject newWeaponPrefab)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        GameObject newWeapon = Instantiate(newWeaponPrefab, transform);
        weapon = newWeapon.transform;
        newWeapon.GetComponent<Weapon>().isFriendlyOwned = true;
    }
}
