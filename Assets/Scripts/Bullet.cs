using UnityEngine;

public class Bullet : MonoBehaviour, IDamager
{
    [SerializeField]
    private float _bulletSpeed = 5;
    [SerializeField]
    private float _lifeTime = 5f;
    private float _bulletTimer = 0f;

    private float _damage = 5f;

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    private float _knockbackStrength;

    public float KnockbackStrength
    {
        get => _knockbackStrength;
        set => _knockbackStrength = value;
    }
    public Vector3 DamagerPosition
    {
        get; set;
    }

    public Bullet(float damage, float knockbackStr, float bulletSpeed, float lifeTime)
    {
        Damage = damage;
        KnockbackStrength = knockbackStr;
        _bulletSpeed = bulletSpeed;
        _lifeTime = lifeTime;
    }

    private void Awake()
    {
        _firedPos = transform.position;
    }

    private Vector3 _firedPos;

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }

    public void SetLifeTime(float time)
    {
        _lifeTime = time;
    }

    public void SetKnockback(float strength)
    {
        KnockbackStrength = strength;
    }
    public void SetDamage(float damage)
    {
        Damage = damage;
    }


    private void OnTriggerEnter(Collider other)
    {

        IHealth HealthComponent = other.gameObject.GetComponent<IHealth>();
        if (HealthComponent != null)
        {
            DamagerPosition = (gameObject.transform.position - _firedPos).normalized;

            //Debug.Log($"Has hit enemy: {other.gameObject.name} for {Damage} damage");

            HealthComponent.OnTakeEnemyDamage(this);

            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime;

        if (_bulletTimer >= _lifeTime)
        {
            Destroy(transform.gameObject);
        }

        _bulletTimer += Time.deltaTime;
    }
}
