
using UnityEngine;

public class BaseShootClass : MonoBehaviour, IShootable
{
    [Header("Override Values")]

    protected float _dmgOverride;
    public float DamageOverride
    {
        get { return _dmgOverride; }
        set { _dmgOverride = value; }
    }

    protected float _knockbackOverride;
    public float KnockbackOverride 
    {
        get {  return _knockbackOverride; }
        set { _knockbackOverride = value; }
    }

    protected float _lifeTimeOverride;

    public float LifeTimeOverride
    {
        get { return _lifeTimeOverride; }
        set { _lifeTimeOverride = value; }
    }

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private AudioSource _shootsfx;
    private void Awake()
    {
        _shootsfx = GetComponent<AudioSource>();
        Debug.Log(_shootsfx.name);
    }

    public virtual void Attack()
    {

        GameObject bulletObj = Instantiate(_bulletPrefab, transform.position, transform.rotation);

        if (_shootsfx != null)
            _shootsfx.Play();

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet == null)
        {
            Debug.LogError("no bullet");
            return;
        }

        bullet.Damage = DamageOverride;
        bullet.KnockbackStrength = KnockbackOverride;
    }

    public virtual  void OnRequestAttack(bool attack)
    {
        
    }
}
