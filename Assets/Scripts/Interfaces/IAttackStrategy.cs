using UnityEngine;

public interface IAttackStrategy
{
    void Aim(Vector3 targetPosition, float rotationSpeed);
    void Attack(Vector3 targetPosition);

}
