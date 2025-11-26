using UnityEngine;

public interface IShootable
{
    void OnRequestAttack(bool attack);
    void Attack();
}
