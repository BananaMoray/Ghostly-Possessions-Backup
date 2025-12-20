using UnityEngine;

public interface IDamager
{

    public float Damage {  get; set; }
    public float Speed {  get; set; }
    public float KnockbackStrength {  get; set; }
    public float LifeTime {  get; set; }
    public Vector3 DamagerPosition {  get; set; }
}
