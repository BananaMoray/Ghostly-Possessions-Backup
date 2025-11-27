using UnityEngine;

public class LeftRightMovementStrategy : IMovementStrategy
{
    private float _distance = 10f;
    private float _speed = 0.5f;
    private Vector3 _startPos;
    private bool _initialized = false;

    public void Move(Transform enemyTransform, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity, Vector3 targetPosition)
    {
        if (!_initialized)
        {
            _startPos = enemyTransform.position;
            _initialized = true;
        }

        Vector3 target = _startPos + Vector3.right * Mathf.Sin(Time.time * _speed) * _distance;
        Vector3 velocity = Vector3.MoveTowards(currentVelocity, (target - enemyTransform.position).normalized * maxSpeed, acceleration * Time.deltaTime);
        rb.linearVelocity = velocity;
    }

    public void Rotate(Transform enemyTransform, Vector3 targetPosition, float rotationSpeed)
    {
        // Optional: always face forward along velocity
        if (enemyTransform.GetComponent<Rigidbody>().linearVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(enemyTransform.GetComponent<Rigidbody>().linearVelocity);
            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}
