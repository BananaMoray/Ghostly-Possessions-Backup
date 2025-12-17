//using UnityEngine;

//public class LeftRightMovementStrategy : MonoBehaviour, IMovementStrategy
//{
//    private float _distance = 10f;
//    private Vector3 _startPos;
//    private bool _initialized = false;

//    public bool Right = true;

//    private void Awake()
//    {
        
//    }


//    private void Update()
//    {



//        //if (transform.position)
//    }

//    public void Move(Vector3 targetPosition, Rigidbody rb, float maxSpeed, float acceleration, float deceleration, Vector3 currentVelocity)
//    {
//        if (!_initialized)
//        {
//            _startPos = transform.position;
//            _initialized = true;
//        }

//        Vector3 target = (Right) ? _startPos + Vector3.right * _distance : _startPos + Vector3.left * _distance;

//        Vector3 velocity = Vector3.MoveTowards(currentVelocity, (target - transform.position).normalized * maxSpeed, acceleration * Time.deltaTime);
//        rb.linearVelocity = velocity;
//    }

//}
