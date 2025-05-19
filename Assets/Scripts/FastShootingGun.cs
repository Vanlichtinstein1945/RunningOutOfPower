using UnityEngine;

public class FastShootingGun : Weapon
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

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().Init(direction, bulletVelocity, isFriendlyOwned);

        lastFireTime = Time.time;
    }
}
