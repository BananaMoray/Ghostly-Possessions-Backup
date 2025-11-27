using UnityEngine;

public interface IShootable
{
    void OnRequestAttack(bool attack);
    void Attack();

    public float DamageOverride
    {
        get; set;
    }
    public float KnockbackOverride { get; set; }
}
