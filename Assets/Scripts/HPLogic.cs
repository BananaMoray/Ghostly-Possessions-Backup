using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HPLogic : MonoBehaviour, IHealth
{
    [SerializeField]
    private float _health = 30f;

    private MeshRenderer _meshRenderer;
    private Material _originMat;

    [SerializeField]
    private Material _damageMat;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.1f;

    private Rigidbody _rb;

    [SerializeField]
    private AudioSource _damageSFX;

    [SerializeField]
    private GameObject _explosionPrefab;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originMat = _meshRenderer.material;
        _rb = GetComponent<Rigidbody>();
    }

    public void SetOriginalColour()
    {
        _originMat = _meshRenderer.material;
    }

    public void OnTakeDamage(IDamager damager)
    {
        _health -= damager.Damage;

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(KnockbackRoutine(damager.DamagerPosition, damager.KnockbackStrength));
            StartCoroutine(TakeDamageFeedback(0.05f));
            HitStopManager.HitStop(0.02f);
        }
    }

    private void Die()
    {
        HitStopManager.HitStop(0.05f);

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        if (explosion != null) Debug.Log("Explosion real");

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
}
