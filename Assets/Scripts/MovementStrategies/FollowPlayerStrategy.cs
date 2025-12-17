using UnityEngine;

public class FollowPlayerStrategy : MonoBehaviour, IMovementStrategy
{
    [SerializeField]
    private float _minDistance = 10f;

    Vector3 newVelocity = Vector3.zero;

    public void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
    {
        //dont do anything if there is no player
        if (targetPosition == Vector3.zero) return;

        Vector3 moveDirection = Vector3.zero;

        moveDirection = (targetPosition - transform.position).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            newVelocity = Vector3.MoveTowards(currentVelocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        else
            newVelocity = Vector3.MoveTowards(currentVelocity, moveDirection * maxSpeed, deceleration * Time.deltaTime);



        rb.linearVelocity = newVelocity;
    }
}
