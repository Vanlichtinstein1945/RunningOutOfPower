using UnityEngine;

public class RicochetBullet : Bullet
{
    public int maxBounces = 3;
    private int bounceCount = 0;

    private void FixedUpdate()
    {
        if (GameManager.instance.isPaused)
        {
            return;
        }

        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - (Vector3)(direction.normalized * 0.1f), direction, 0.2f, obstacleLayer);
            if (hit.collider != null)
            {
                direction = Vector2.Reflect(direction, hit.normal);

                if (bounceCount >= maxBounces)
                {
                    Destroy(gameObject);
                }
                bounceCount++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag == "Enemy" && isFriendlyBullet)
        {
            collision.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player" && !isFriendlyBullet)
        {
            collision.gameObject.GetComponent<PlayerController>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
