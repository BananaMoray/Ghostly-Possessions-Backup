using UnityEngine;

public class FollowPlayerStrategy : MonoBehaviour, IMovementStrategy
{
    [SerializeField]
    private float _minDistance = 10f;

    public void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
    {
        //dont do anything if there is no player
        if (targetPosition == Vector3.zero) return;

        Vector3 moveDirection = Vector3.zero;

        //i have to square my mininum distance here, since the magnitude is also squared
        if ((targetPosition - transform.position).sqrMagnitude > _minDistance * _minDistance)
        {
            moveDirection = (targetPosition - transform.position).normalized;
        }
        else
            moveDirection = Vector3.zero;

        Vector3 velocity = Vector3.MoveTowards(currentVelocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
        rb.linearVelocity = velocity;
    }
}
