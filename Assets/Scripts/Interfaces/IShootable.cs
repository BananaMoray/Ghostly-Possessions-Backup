using UnityEngine;

public interface IShootable
{
    void OnRequestAttack(bool attack);
    void Attack();
    void Attack(int amount, float angle);

    public float DamageOverride
    {
        get; set;
    }
    public float KnockbackOverride { get; set; }
    public float LifeTimeOverride { get; set; }
    public float SpeedOverride { get; set; }
    public float AttackDelayOverride { get; set; }
}
