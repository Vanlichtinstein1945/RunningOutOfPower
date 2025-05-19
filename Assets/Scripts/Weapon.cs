using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float bulletsPerSecond = 1f;
    protected float lastFireTime = -Mathf.Infinity;
    public float bulletVelocity = 8f;
    public GameObject bulletPrefab;
    public bool isFriendlyOwned;
    public string weaponName;

    public abstract void Shoot(Vector2 direction = default);
}
