using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPLogic : MonoBehaviour, IHealth
{
    public event Action OnDied;

    [Header("HP Values")]
    [SerializeField]
    protected float _maxHealth = 30f;
    protected float _currentHealth;


    public float HpDrainRateInSeconds = 1f;
    private float _drainTimer;

    //rigidbody
    protected Rigidbody _rb;

    [Header("Prefabs")]
    [SerializeField]
    protected AudioSource _damageSFX;

    //material values
    [SerializeField]
    private Material _damageMat;
    private MeshRenderer _meshRenderer;
    private Material _originMat;

    [SerializeField]
    protected GameObject _explosionPrefab;

    ////HPBar Stuff
    //[SerializeField]
    //private GameObject _hpBarPrefab;
    //private Slider _hpBarSlider;

    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    //public GameObject HPBar
    //{
    //    get { return _hpBarPrefab; }
    //    set { _hpBarPrefab = value; }
    //}

    private bool _healthDrainEnabled;

    public bool HealthDrainEnabled
    {
        get { return _healthDrainEnabled; }
        set { _healthDrainEnabled = value; }
    }

    protected virtual void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        if (_meshRenderer != null)
            _originMat = _meshRenderer.material;

        _rb = GetComponent<Rigidbody>();

        _currentHealth = MaxHealth;

        //if (_hpBarPrefab != null)
        //{
        //    InstantiateHPBar();
        //}
    }

    //protected virtual void InstantiateHPBar()
    //{
    //    HPBar = Instantiate(_hpBarPrefab, transform.position, new Quaternion(90, 0, 0, 0));
    //    _hpBarSlider = HPBar.GetComponentInChildren<Slider>();
    //    HPBar.GetComponent<HPBarController>().owner = gameObject;
    //    SetHPBarActive(false);
    //}

    public virtual void SetHPBarActive(bool b)
    {

    }

    protected virtual void Update()
    {
        if (HealthDrainEnabled)
        {
            _drainTimer += Time.deltaTime;
            if (_drainTimer >= HpDrainRateInSeconds)
            {
                TakeDamage(1f);
                _drainTimer = 0;
                //Debug.Log("Health drained");
            }
        }
    }

    public virtual void SetOriginalColour()
    {
        _originMat = _meshRenderer.material;
    }

    protected virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }

        //if (HPBar != null)
        //    _hpBarSlider.value = GetCurrentHealthPercent();

        //Debug.Log($"{this.gameObject.name} current HP%: {GetCurrentHealthPercent() * 100}%, Current HP: {_health}/{MaxHealth}");
    }

    public virtual void OnTakeEnemyDamage(IDamager damager)
    {
        TakeDamage(damager.Damage);
        HitStopManager.HitStop(0.02f);

        if (_currentHealth > 0)
        {
            StartCoroutine(KnockbackRoutine(damager.DamagerPosition, damager.KnockbackStrength));
            StartCoroutine(TakeDamageFeedback(0.05f));
        }

    }

    protected virtual void Die()
    {
        HitStopManager.HitStop(0.05f);

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        //if (explosion != null) Debug.Log("Explosion real");

        OnDied?.Invoke();

        //Destroy(HPBar);
        Destroy(gameObject);
    }

    public IEnumerator TakeDamageFeedback(float seconds)
    {
        _meshRenderer.material = _damageMat;

        if (_damageSFX != null)
            _damageSFX.Play();

        yield return new WaitForSeconds(seconds);

        _meshRenderer.material = _originMat;
    }

    protected IEnumerator KnockbackRoutine(Vector3 pos, float KnockbackForce)
    {
        if (!_rb) yield break;

        Vector3 direction = (pos).normalized;

        //_rb.linearVelocity = Vector3.zero;

        _rb.AddForce(direction * KnockbackForce, ForceMode.Impulse);
    }

    public float GetCurrentHealthPercent()
    {
        return _currentHealth / MaxHealth;
    }
}
