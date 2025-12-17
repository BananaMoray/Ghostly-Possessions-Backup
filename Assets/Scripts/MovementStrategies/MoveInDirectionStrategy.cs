//using UnityEngine;

//public class MoveInDirectionStrategy : MonoBehaviour, IMovementStrategy
//{
//    [SerializeField]
//    private float _minDistance = 10f;

//    Vector3 newVelocity = Vector3.zero;

//    public void Move(Vector3 moveDirection, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
//    {

//        if (moveDirection.sqrMagnitude > 0.01f)
//        {
//            newVelocity = Vector3.MoveTowards(currentVelocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
//        }
//        else
//            newVelocity = Vector3.MoveTowards(currentVelocity, moveDirection * maxSpeed, deceleration * Time.deltaTime);

//        rb.linearVelocity = newVelocity;
//    }
//}
