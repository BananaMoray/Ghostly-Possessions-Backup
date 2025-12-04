using UnityEngine;
using UnityEngine.UI;

public interface IHealth 
{

    public float MaxHealth {  get; set; }
    public bool HealthDrainEnabled {  get; set; }
    public void OnTakeEnemyDamage(IDamager damager);
    public float GetCurrentHealthPercent();
    public void SetOriginalColour();
    public void SetHPBarActive(bool b);
}
