using UnityEngine;

public interface IHealth 
{
    void OnTakeDamage(IDamager damager);
    void SetOriginalColour();
}
