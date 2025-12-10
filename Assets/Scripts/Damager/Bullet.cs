using UnityEngine;

public class Bullet : MonoBehaviour, IDamager
{
    [SerializeField]
    private float _bulletSpeed = 5;
    [SerializeField]
    public float LifeTime = 5f;
    private float _bulletTimer = 0f;

    private float _damage = 5f;

    public float Speed
    {
        get { return _bulletSpeed; }
        set { _bulletSpeed = value; }
    }

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
        Speed = bulletSpeed;
        LifeTime = lifeTime;
    }

    private void Awake()
    {
        _firedPosition = transform.position;
    }

    private Vector3 _firedPosition;


    private void OnTriggerEnter(Collider other)
    {

        IHealth HealthComponent = other.gameObject.GetComponent<IHealth>();
        if (HealthComponent != null)
        {
            DamagerPosition = (gameObject.transform.position - _firedPosition).normalized;

            //Debug.Log($"Has hit enemy: {other.gameObject.name} for {Damage} damage");

            HealthComponent.OnTakeEnemyDamage(this);

            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;

        if (_bulletTimer >= LifeTime)
        {
            Destroy(transform.gameObject);
        }

        _bulletTimer += Time.deltaTime;
    }
}
