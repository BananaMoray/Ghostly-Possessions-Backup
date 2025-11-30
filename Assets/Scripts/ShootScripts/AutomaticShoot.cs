using System.Collections;
using UnityEngine;

public class AutomaticShoot : BaseShootClass
{
    [SerializeField]
    private float _damage = 10f;
    [SerializeField]
    private float _knockbackStrength = 2f;
    [SerializeField]
    private float _lifetime = 2f;

    public float _shootDelay = 0.1f;
    private bool _canAttack = true;


    private void Awake()
    {
        DamageOverride = _damage;
        KnockbackOverride = _knockbackStrength;
        LifeTimeOverride = _lifetime;
    }

    public override void OnRequestAttack(bool attack)
    {
        if (attack && _canAttack)
            StartCoroutine(AttackWithDelay(_shootDelay));
    }

    public IEnumerator AttackWithDelay(float delay)
    {
        _canAttack = false;
        Attack();
        yield return new WaitForSeconds(delay);
        _canAttack = true;
    }

    public override void Attack()
    {
        base.Attack();

    }
}
