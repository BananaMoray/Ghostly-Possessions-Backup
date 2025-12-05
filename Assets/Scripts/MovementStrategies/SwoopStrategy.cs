using UnityEngine;

public class SwoopStrategy : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float _swoopDistance = 8f;
    [SerializeField] private float _retreatDistance = 12f;
    [SerializeField] private float _retreatTime = 1.5f;

    private bool _retreating = false;
    private float _retreatTimer = 0f;

    public void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
    {
        if (targetPosition == Vector3.zero) return;

        if (_retreating)
        {
            _retreatTimer += Time.deltaTime;

            Vector3 retreatDir = (transform.position - targetPosition).normalized;
            Vector3 retreatVel = Vector3.MoveTowards(currentVelocity, retreatDir * maxSpeed, acceleration * Time.deltaTime);

            rb.linearVelocity = retreatVel;

            if (_retreatTimer >= _retreatTime)
            {
                _retreating = false;
                _retreatTimer = 0f;
            }

            return;
        }

        float sqrDist = (targetPosition - transform.position).sqrMagnitude;

        if (sqrDist <= _swoopDistance * _swoopDistance)
        {
            _retreating = true;
            return;
        }

        Vector3 dir = (targetPosition - transform.position).normalized;
        Vector3 desiredVel = dir * maxSpeed;

        rb.linearVelocity = Vector3.MoveTowards(currentVelocity, desiredVel, acceleration * Time.deltaTime);
    }
}
