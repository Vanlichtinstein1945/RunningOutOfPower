using UnityEngine;

public class FanShootingGun : Weapon
{
    public int bulletCount = 3;
    public float spreadAngle = 15f;

    public override void Shoot(Vector2 direction = default)
    {
        if (Time.time < lastFireTime + 1f / bulletsPerSecond)
        {
            return;
        }

        direction = transform.right.normalized;

        float angleStep = (bulletCount > 1) ? spreadAngle / (bulletCount - 1) : 0f;
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = startAngle + i * angleStep;
            Vector2 angledDirection = Quaternion.Euler(0f, 0f, angleOffset) * direction;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Init(angledDirection, bulletVelocity, isFriendlyOwned);
        }

        lastFireTime = Time.time;
    }
}
