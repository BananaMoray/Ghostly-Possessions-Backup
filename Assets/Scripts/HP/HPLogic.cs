using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HPLogic : MonoBehaviour, IHealth
{
    public event Action OnDied;

    [Header("Variables")]
    [SerializeField]
    protected float _maxHealth = 30f;
    protected float _currentHealth;

    public float HpDrainRateInSeconds = 1f;
    private float _drainTimer;

    //rigidbody
    protected Rigidbody _rb;

    [Header("Prefabs")]
    [SerializeField]
    protected AudioResource _damageSFX;

    //material values
    [SerializeField]
    protected Material _damageMat;
    protected MeshRenderer _meshRenderer;
    protected Material _originMat;

    [SerializeField]
    protected GameObject _explosionPrefab;

    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

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
    }


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
            ExplodeOrInactivate();
        }
    }

    protected virtual void HealDamage(float health)
    {
        _currentHealth += health;
    }

    public virtual void ResetHealth()
    {
        _currentHealth = MaxHealth;
    }

    private void ExplodeOrInactivate()
    {
        ExplodeOnDeath();
    }

    public virtual void OnTakeEnemyDamage(IDamager damager)
    {
        TakeDamage(damager.Damage);

        if (_currentHealth > 0)
        {
            StartCoroutine(KnockbackRoutine(damager.DamagerPosition, damager.KnockbackStrength));
            StartCoroutine(TakeDamageFeedback(0.05f));
        }

    }

    protected virtual void ExplodeOnDeath()
    {
        StartCoroutine(ExplosionDelayRoutine());


    }

    public IEnumerator TakeDamageFeedback(float seconds)
    {
        _meshRenderer.material = _damageMat;

        if (_damageSFX != null)
            SoundManager.Instance.PlaySoundFXClip(_damageSFX, transform, SoundManager.SFXVolume);

        yield return new WaitForSeconds(seconds);

        _meshRenderer.material = _originMat;

        HitStopManager.HitStop(0.02f);
    }

    protected IEnumerator KnockbackRoutine(Vector3 pos, float KnockbackForce)
    {
        if (!_rb) yield break;

        Vector3 direction = pos;

        //_rb.linearVelocity = Vector3.zero;

        _rb.AddForce(direction * KnockbackForce, ForceMode.Impulse);
    }

    public IEnumerator ExplosionDelayRoutine()
    {
        _meshRenderer.material = _damageMat;

        yield return new WaitForSeconds(.5f);

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        //if (explosion != null) Debug.Log("Explosion real");

        OnDied?.Invoke();

        //Destroy(HPBar);
        Destroy(gameObject);
    }

    public float GetCurrentHealthPercent()
    {
        return _currentHealth / MaxHealth;
    }
}
