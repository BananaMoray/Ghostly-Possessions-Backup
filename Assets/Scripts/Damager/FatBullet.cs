using System.Collections;
using UnityEngine;

public class FatBullet : MonoBehaviour, IDamager
{
    [SerializeField]
    private float _bulletSpeed = 5;

    private float _lifeTimer = 0f;

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

    private float _lifeTime;

    public float LifeTime
    {
        get => _lifeTime;
        set => _lifeTime = value;
    }

    public Vector3 DamagerPosition
    {
        get; set;
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

            //Destroy(this.gameObject);
        }
    }

    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;

        //if (_lifeTimer >= LifeTime)
        //{
        //    Destroy(transform.gameObject);
        //}

        if (_lifeTimer >= LifeTime && !_isShrinking)
        {
            StartCoroutine(KillBullet(0.25f));
        }

        _lifeTimer += Time.deltaTime;

        if (_isShrinking)
        {
            GetComponent<Collider>().enabled = false;

            Vector3 targetScale = new Vector3(0, 0, 0);

            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, 6f * Time.deltaTime);
        }
    }

    private bool _isShrinking;

    public IEnumerator KillBullet(float delay)
    {
        _isShrinking = true;

        yield return new WaitForSeconds(delay);

        Destroy(transform.gameObject);
    }
}
