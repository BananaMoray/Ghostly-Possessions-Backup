
using UnityEngine;

public class BaseShootClass : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private AudioSource _shootsfx;

    [Header("Override Values")]

    [SerializeField]
    protected float _dmgOverride;
    public float DamageOverride
    {
        get { return _dmgOverride; }
        set { _dmgOverride = value; }
    }

    [SerializeField]
    protected float _knockbackOverride;
    public float KnockbackOverride
    {
        get { return _knockbackOverride; }
        set { _knockbackOverride = value; }
    }

    [SerializeField]
    protected float _speedOverride;
    public float SpeedOverride
    {
        get { return _speedOverride; }
        set { _speedOverride = value; }
    }

    [SerializeField]
    protected float _lifeTimeOverride;
    public float LifeTimeOverride
    {
        get { return _lifeTimeOverride; }
        set { _lifeTimeOverride = value; }
    }

    private void Awake()
    {
        _shootsfx = GetComponent<AudioSource>();
        Debug.Log(_shootsfx.name);
    }

    public virtual void Attack()
    {
        SpawnBullet(transform.rotation);
    }

    public virtual void Attack(int amount, float angle)
    {
        float halfAngle = angle / 2;

        for (int i = 0; i < amount; i++)
        {

            float t;

            //first bullet is always the most left bullet
            if (amount == 1)
            {
                t = 0;
            }
            else
            {
                t = (float) i / (amount - 1);
            }

            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);

            Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, currentAngle, 0));

            SpawnBullet(rotation);
        }
    }

    public GameObject SpawnBullet(Quaternion rot)
    {
        GameObject bulletObj = Instantiate(_bulletPrefab, transform.position, rot);

        if (_shootsfx != null)
            _shootsfx.Play();

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet == null)
        {
            Debug.LogError("no bullet!");
            return null;
        }

        bullet.Damage = DamageOverride;
        bullet.KnockbackStrength = KnockbackOverride;
        bullet.LifeTime = LifeTimeOverride;
        bullet.Speed = SpeedOverride;

        return bulletObj;
    }


    public virtual void OnRequestAttack(bool attack)
    {

    }
}
