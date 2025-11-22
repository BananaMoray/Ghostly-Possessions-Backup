using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BasicEnemyLogic : MonoBehaviour, IEnemy
{
    [SerializeField]
    private float _health = 30f;

    public void OnAttack()
    {
        
    }

    public void OnTakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
