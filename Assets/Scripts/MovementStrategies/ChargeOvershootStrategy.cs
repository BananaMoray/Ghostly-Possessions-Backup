using UnityEngine;

public class ChargeOvershootStrategy : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float _rushAccelerationMultiplier = 2.5f;
    [SerializeField] private float _slowDownMultiplier = 0.4f;

    public void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
    {
        if (targetPosition == Vector3.zero) return;

        // Charge aggressively toward player
        Vector3 dir = (targetPosition - transform.position).normalized;

        float accel = acceleration * _rushAccelerationMultiplier;
        Vector3 targetVel = dir * maxSpeed;

        Vector3 newVel = Vector3.MoveTowards(currentVelocity, targetVel, accel * Time.deltaTime);

        // Weak braking -> overshoot effect
        if (Vector3.Dot(dir, currentVelocity) < 0f)
        {
            newVel = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * _slowDownMultiplier * Time.deltaTime);
        }

        rb.linearVelocity = newVel;
    }
}
