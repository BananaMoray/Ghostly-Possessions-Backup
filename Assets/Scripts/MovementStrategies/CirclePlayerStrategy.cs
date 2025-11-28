using UnityEngine;

[System.Serializable]
public class CirclePlayerStrategy : MonoBehaviour, IMovementStrategy
{
    public void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
    {
        throw new System.NotImplementedException();
    }
}
