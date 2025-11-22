using UnityEngine;

public class BasicShootController : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private float _damage = 10f;

    public void OnShoot()
    {
        ShootBullet();
    }

    public void ShootBullet()
    {
        BulletController bulletController = _bullet.GetComponent<BulletController>();

        bulletController.BulletDamage = _damage;

        Instantiate(_bullet, transform.position, transform.rotation);
    }
}
