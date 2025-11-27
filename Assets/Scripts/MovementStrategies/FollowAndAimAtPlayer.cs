using UnityEngine;

public class FollowAndAimAtPlayer : IMovementStrategy
{
    private float _minDistance = 10f;

    public void Move(Transform enemyTransform, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity, Vector3 targetPosition)
    {
        if (targetPosition == Vector3.zero) return;

        Vector3 toPlayer = targetPosition - enemyTransform.position;
        float sqrDistance = toPlayer.sqrMagnitude;

        Vector3 direction = Vector3.zero;

        if (sqrDistance > _minDistance * _minDistance)
        {
            direction = toPlayer.normalized;
        }
        else
        {
            direction = Vector3.zero;
        }

        Vector3 velocity = Vector3.MoveTowards(currentVelocity, direction * maxSpeed, acceleration * Time.deltaTime);
        rb.linearVelocity = velocity;
    }

    public void Rotate(Transform enemyTransform, Vector3 targetPosition, float rotationSpeed)
    {
        if (targetPosition == Vector3.zero) return;

        Vector3 direction = targetPosition - enemyTransform.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}
