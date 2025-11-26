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

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originColor = _meshRenderer.material.color;
    }

    public void OnAttack()
    {
        
    }

    public void OnTakeDamage(float damage)
    {
        StartCoroutine(TakeDamage(0.1f));

        _health -= damage;

        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public IEnumerator TakeDamage(float seconds)
    {
        _meshRenderer.material.color = Color.white;
        yield return new WaitForSeconds(seconds);
        _meshRenderer.material.color = _originColor;
    }
}
