using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HPLogic : MonoBehaviour, IHealth
{
    public event Action OnDied;

    [Header("HP Values")]
    [SerializeField]
    private float _maxHealth = 30f;
    private float _currentHealth;

    [SerializeField]
    private float _hpDrainRateInSeconds = 1f;
    private float _currentHPDrainTimer;

    //rigidbody
    private Rigidbody _rb;

    [Header("Prefabs")]
    [SerializeField]
    private AudioSource _damageSFX;

    //material values
    [SerializeField]
    private Material _damageMat;
    private MeshRenderer _meshRenderer;
    private Material _originMat;

    [SerializeField]
    private GameObject _explosionPrefab;

    //HPBar Stuff
    [SerializeField]
    private GameObject _hpBarPrefab;
    private Slider _hpBarSlider;

    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public GameObject HPBar
    {
        get { return _hpBarPrefab; }
        set { _hpBarPrefab = value; }
    }

    private bool _healthDrainEnabled;

    public bool HealthDrainEnabled
    {
        get { return _healthDrainEnabled; }
        set { _healthDrainEnabled = value; }
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originMat = _meshRenderer.material;
        _rb = GetComponent<Rigidbody>();
        _currentHealth = MaxHealth;

        if (_hpBarPrefab != null)
        {
            InstantiateHPBar();
        }
    }

    private void InstantiateHPBar()
    {
        HPBar = Instantiate(_hpBarPrefab, transform.position, new Quaternion(90, 0, 0, 0));
        _hpBarSlider = HPBar.GetComponentInChildren<Slider>();
        HPBar.GetComponent<HPBarController>().owner = gameObject;
        SetHPBarActive(false);
    }

    public void SetHPBarActive(bool b)
    {
        if (HPBar != null)
        {
            HPBar.SetActive(b);
        }
    }

    private void Update()
    {
        if (HealthDrainEnabled)
        {
            _currentHPDrainTimer += Time.deltaTime;
            if (_currentHPDrainTimer >= _hpDrainRateInSeconds)
            {
                TakeDamage(1f);
                _currentHPDrainTimer = 0;
                //Debug.Log("Health drained");
            }
        }
    }

    public void SetOriginalColour()
    {
        _originMat = _meshRenderer.material;
    }

    private void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }

        if (HPBar != null)
            _hpBarSlider.value = GetCurrentHealthPercent();

        //Debug.Log($"{this.gameObject.name} current HP%: {GetCurrentHealthPercent() * 100}%, Current HP: {_health}/{MaxHealth}");
    }

    public void OnTakeEnemyDamage(IDamager damager)
    {
        TakeDamage(damager.Damage);

        StartCoroutine(KnockbackRoutine(damager.DamagerPosition, damager.KnockbackStrength));
        StartCoroutine(TakeDamageFeedback(0.05f));
        HitStopManager.HitStop(0.02f);

    }

    private void Die()
    {
        HitStopManager.HitStop(0.05f);

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        //if (explosion != null) Debug.Log("Explosion real");

        OnDied?.Invoke();

        Destroy(HPBar);
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

    private IEnumerator KnockbackRoutine(Vector3 pos, float KnockbackForce)
    {
        if (!_rb) yield break;

        Vector3 direction = (pos).normalized;

        _rb.AddForce(direction * KnockbackForce, ForceMode.Impulse);
    }

    public float GetCurrentHealthPercent()
    {
        return _currentHealth / MaxHealth;
    }
}
