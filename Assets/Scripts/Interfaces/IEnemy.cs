using UnityEngine;

public interface IEnemy 
{
    void OnTakeDamage(IDamager damager);

    void OnAttack();
}
