using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 8f;
    [SerializeField] private float rotationSpeed = 360f;

    private Rigidbody _rb;
    private Vector3 _currentVelocity = Vector3.zero;

    public GameObject _player;

    public IMovementStrategy MovementStrategy;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb != null)
            _rb.freezeRotation = true;

        _player = GameObject.FindGameObjectWithTag("Player");

    }

    private void FixedUpdate()
    {
        if (MovementStrategy == null) return;

        Vector3 targetPos = _player != null ? _player.transform.position : Vector3.zero;

        MovementStrategy.Move(transform, _rb, maxSpeed, acceleration, deceleration, _currentVelocity, targetPos);

        MovementStrategy.Rotate(transform, targetPos, rotationSpeed);

        _currentVelocity = _rb.linearVelocity;
    }
}
