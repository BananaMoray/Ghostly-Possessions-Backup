using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float _bulletSpeed = 5;
    [SerializeField]
    private float _lifeTime = 5f;
    private float _bulletTimer = 0f;
    public float BulletDamage = 5f;

    public BulletController (float bulletSpeed, float lifeTime, float bulletDamage)
    {
        _bulletSpeed = bulletSpeed;
        _lifeTime = lifeTime;
        BulletDamage = bulletDamage;
    }

    private void Awake()
    {
        //Debug.Log("bullet damage: " + BulletDamage);
    }

    public BulletController(float bulletDamage)
    {
        BulletDamage = bulletDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        

        IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
        if (enemy != null)
        {
            Debug.Log($"Has hit enemy: {other.gameObject.name} for {BulletDamage} damage");

            enemy.OnTakeDamage(BulletDamage);

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
