using UnityEngine;

public class NormalGun : Weapon
{
    public override void Shoot(Vector2 direction)
    {
        if (Time.time < lastFireTime + 1f / bulletsPerSecond)
        {
            return;
        }

        if (direction == Vector2.zero)
        {
            direction = transform.right.normalized;
        }

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(direction, bulletVelocity, isFriendlyOwned);

        lastFireTime = Time.time;
    }
}
