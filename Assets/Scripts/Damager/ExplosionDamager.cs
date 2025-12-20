using UnityEngine;

public class ExplosionDamager : MonoBehaviour, IDamager
{
    private float _speed = 0;

    [SerializeField]
    private float _damage = 25f;

    [SerializeField]
    private float _knockbackStrength = 10f;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (!_damageEnabled) return;

        IHealth HealthComponent = other.gameObject.GetComponent<IHealth>();
        if (HealthComponent != null)
        {
            DamagerPosition = (other.gameObject.transform.position - gameObject.transform.position).normalized;

            HealthComponent.OnTakeEnemyDamage(this);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 2f);
    }

    private float _explosionTimer;
    private bool _damageEnabled = true;

    private void FixedUpdate()
    {
        if (_explosionTimer >= 0.1f)
            _damageEnabled = false;
        _explosionTimer += Time.deltaTime;
    }

}
