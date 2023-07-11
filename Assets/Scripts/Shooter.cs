using UnityEngine;

public class Shooter : MonoBehaviour
{
    private BulletObjectPool bulletPool;
    [SerializeField] private float projectileOffsetMultiplier;

    private void Start()
    {
        bulletPool = BulletObjectPool.Instance;
    }

    public void Shoot(Vector3 target)
    {
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = transform.position + target * projectileOffsetMultiplier;
        bullet.transform.eulerAngles = transform.position - target;
        bullet.SetActive(true);
        bullet.GetComponent<Projectile>().SetTarget(target);
    }
}
