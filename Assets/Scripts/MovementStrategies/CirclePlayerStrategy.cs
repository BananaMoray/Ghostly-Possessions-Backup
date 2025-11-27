using UnityEngine;

[System.Serializable]
public class CirclePlayerStrategy : IMovementStrategy
{
    public float radius = 15f;      // Distance from player
    public float speed = 0.5f;      // Orbit speed (radians/sec)
    public bool useRandomStartAngle = true; // Use random start angle?

    private bool _initialized = false;
    private float _angle = 0f;

    public void Move(Transform enemyTransform, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity, Vector3 targetPosition)
    {
        if (targetPosition == Vector3.zero) return; // No player reference

        // Initialize starting angle
        if (!_initialized)
        {
            if (useRandomStartAngle)
            {
                _angle = Random.Range(0f, Mathf.PI * 2f);
            }
            else
            {
                // Calculate angle based on current position relative to player
                Vector3 toEnemy = enemyTransform.position - targetPosition;
                _angle = Mathf.Atan2(toEnemy.z, toEnemy.x); // radians
            }

            _initialized = true;
        }

        // Increment angle for circular movement
        _angle += speed * Time.deltaTime;

        // Calculate position offset on circle
        Vector3 offset = new Vector3(Mathf.Cos(_angle), 0, Mathf.Sin(_angle)) * radius;
        Vector3 target = targetPosition + offset;

        // Move smoothly toward target
        Vector3 velocity = Vector3.MoveTowards(currentVelocity, (target - enemyTransform.position).normalized * maxSpeed, acceleration * Time.deltaTime);
        rb.linearVelocity = velocity;
    }

    public void Rotate(Transform enemyTransform, Vector3 targetPosition, float rotationSpeed)
    {
        if (targetPosition == Vector3.zero) return;

        Vector3 dir = targetPosition - enemyTransform.position;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}
