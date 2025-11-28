using UnityEngine;

public class TapShoot : BaseShootClass
{
    [SerializeField]
    private float _damage = 10f;
    [SerializeField]
    private float _knockbackStr = 5f;

    private bool _previousAttack;

    private void Awake()
    {
        // Apply stats to parent overrides
        DamageOverride = _damage;
        KnockbackOverride = _knockbackStr;
    }

    public override void OnRequestAttack(bool attack)
    {
        if(attack && _previousAttack == false)
            Attack();

        _previousAttack = attack;
    }

    public override void Attack()
    {
        base.Attack();
        //Bullet bullet = _bullet.GetComponent<Bullet>();

        //bullet.SetDamage(_damage);
        //bullet.SetKnockback(_knockbackStr);

        //Instantiate(_bullet, transform.position, transform.rotation);
    }
}
