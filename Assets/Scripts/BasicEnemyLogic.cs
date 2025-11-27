using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BasicEnemyLogic : MonoBehaviour, IEnemy
{
    [SerializeField]
    private float _health = 30f;

    private MeshRenderer _meshRenderer;
    private Color _originColor;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.1f;

    private Rigidbody _rb;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originColor = _meshRenderer.material.color;
        _rb = GetComponent<Rigidbody>();
    }

    public void OnAttack()
    {
        
    }

    public void OnTakeDamage(IDamager damager)
    {
        _health -= damager.Damage;

        StartCoroutine(TakeDamageFlash(0.05f));

        StartCoroutine(KnockbackRoutine(damager.DamagerPosition, damager.KnockbackStrength));

        HitStopManager.HitStop(0.05f);

        if (_health <= 0)
            Destroy(gameObject);
            //gameObject.SetActive(false);
    }

    public IEnumerator TakeDamageFlash(float seconds)
    {
        _meshRenderer.material.color = Color.white;

        yield return new WaitForSeconds(seconds);

        _meshRenderer.material.color = _originColor;

    }

    private IEnumerator KnockbackRoutine(Vector3 pos, float KnockbackForce)
    {
        if (!_rb) yield break;
        

        Vector3 direction = (transform.position - pos).normalized;  
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            timer += Time.unscaledDeltaTime;
            _rb.AddForce(direction * KnockbackForce, ForceMode.Impulse);
            yield return null;
        }

        _rb.linearVelocity = Vector3.zero;
    }
}
