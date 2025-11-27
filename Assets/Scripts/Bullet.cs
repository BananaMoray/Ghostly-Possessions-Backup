using UnityEngine;

public class Bullet : MonoBehaviour, IDamager
{
    [SerializeField]
    private float _bulletSpeed = 5;
    [SerializeField]
    private float _lifeTime = 5f;
    private float _bulletTimer = 0f;

    [SerializeField]
    private float _damage = 5f;

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField]
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
        //Debug.Log("bullet damage: " + BulletDamage);
    }

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
        

        IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
        if (enemy != null)
        {
            DamagerPosition = gameObject.transform.position;

            Debug.Log($"Has hit enemy: {other.gameObject.name} for {Damage} damage");

            enemy.OnTakeDamage(this);

            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime;

        if (_bulletTimer >= _lifeTime)
        {
            //Debug.Log("Goodbye Bullet");
            Destroy(transform.gameObject);

        }

        _bulletTimer += Time.deltaTime;
    }
}
