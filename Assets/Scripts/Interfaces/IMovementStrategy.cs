using UnityEngine;

public interface IMovementStrategy
{
    void Move(Transform enemyTransform, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity, Vector3 targetPosition);
    void Rotate(Transform enemyTransform, Vector3 targetPosition, float rotationSpeed);
}

