
using UnityEngine;

public class BaseShootClass : MonoBehaviour, IShootable
{
    [Header("Override Values")]
    [SerializeField]
    private float _dmgOverride = 5f;
    public float DamageOverride
    {
        get { return _dmgOverride; }
        set { _dmgOverride = value; }
    }

    [SerializeField]
    private float _knockbackOverride = 5f;
    public float KnockbackOverride 
    {
        get {  return _knockbackOverride; }
        set { _knockbackOverride = value; }
    }

    [SerializeField]
    private GameObject _bulletPrefab;

    public virtual void Attack()
    {

        GameObject bulletObj = Instantiate(_bulletPrefab, transform.position, transform.rotation);


        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet == null)
        {
            Debug.LogError("no bullet");
            return;
        }

        bullet.Damage = DamageOverride;
        bullet.KnockbackStrength = KnockbackOverride;

        Collider bulletCollider = bulletObj.GetComponent<Collider>();
        Collider shooterCollider = GetComponent<Collider>();
        if (bulletCollider != null && shooterCollider != null)
            Physics.IgnoreCollision(bulletCollider, shooterCollider);
    }

    public virtual  void OnRequestAttack(bool attack)
    {
        
    }
}
