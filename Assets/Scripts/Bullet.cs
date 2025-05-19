using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public int damage = 10;
    public LayerMask obstacleLayer;
    public float maxDistance = 100f;

    protected float speed;
    protected Vector2 direction;
    protected Vector3 spawnPosition;
    protected Rigidbody2D rb;
    protected bool isFriendlyBullet;

    public virtual void Init(Vector2 direction, float velocity, bool isFriendly)
    {
        rb = GetComponent<Rigidbody2D>();
        this.direction = direction;
        speed = velocity;
        isFriendlyBullet = isFriendly;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void Start()
    {
        spawnPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (GameManager.instance.isPaused)
        {
            return;
        }

        CheckDistance();
    }

    protected void CheckDistance()
    {
        if (Vector3.SqrMagnitude(transform.position - spawnPosition) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
