using UnityEngine;

public class NormalBullet : Bullet
{
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
            Destroy(gameObject);
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
