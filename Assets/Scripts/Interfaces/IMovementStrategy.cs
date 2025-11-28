using UnityEngine;

public interface IMovementStrategy
{
    void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity);

}

