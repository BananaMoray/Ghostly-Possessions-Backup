using UnityEngine;

public interface IMovementStrategy
{
    //Rigidbody Rigidbody { get; set; }

    //float MaxSpeed { get; set; }

    //float Acceleration {  get; set; }

    //float Deceleration {  get; set; }

    //Vector3 CurrentVelocity { get; set; }

    void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity);

}

