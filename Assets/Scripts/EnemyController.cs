using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] 
    private float _maxSpeed = 3f;
    [SerializeField] 
    private float _acceleration = 20f;
    [SerializeField] 
    private float _deceleration = 8f;
    [SerializeField] 
    private float _rotationSpeed = 360f;

    private Rigidbody _rb;
    private Vector3 _currentVelocity = Vector3.zero;

    public GameObject _player;

    [Header("Strategies")]
    public MonoBehaviour MovementStrategy;
    public MonoBehaviour RotationStrategy;

    private IMovementStrategy _movementStrategy;
    private IRotationStrategy _rotationStrategy;

    private IShootable _shootComponent;
    [SerializeField]
    private float _validShootDistance = 12f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb != null)
            _rb.freezeRotation = true;

        _player = GameObject.FindGameObjectWithTag("Player");

        if (MovementStrategy == null)
            Debug.LogError($"Movement Strategy not implemented for {gameObject.name}");
        else
            _movementStrategy = MovementStrategy as IMovementStrategy;

        if (RotationStrategy == null)
            Debug.LogError($"Rotation Strategy not implemented for {gameObject.name}");
        else
            _rotationStrategy = RotationStrategy as IRotationStrategy;

        _shootComponent = GetComponent<IShootable>();
        if (_shootComponent == null)
            Debug.LogError($"Shoot Component not implemented for {gameObject.name}");
    }

    private void FixedUpdate()
    {
        if (MovementStrategy == null) return;

        Vector3 targetPos = _player != null ? _player.transform.position : Vector3.zero;

        _movementStrategy.Move(targetPos, _rb, _maxSpeed, _acceleration, _deceleration, _currentVelocity);

        _rotationStrategy.Rotate(targetPos, _rotationSpeed);

        _currentVelocity = _rb.linearVelocity;

        if (_shootComponent != null)
        {
            _shootComponent.OnRequestAttack(IsPlayerInRange(targetPos));
        }

    }

    private bool IsPlayerInRange(Vector3 targetPos)
    {
        if (targetPos == Vector3.zero) return false;

        return ((targetPos - transform.position).sqrMagnitude <= _validShootDistance * _validShootDistance);
    }

    public void ChangeMovementStrategy(IMovementStrategy movementStrategy)
    {
        _movementStrategy = movementStrategy;
    }
    public void ChangeRotationStrategy(IRotationStrategy rotationStrategy)
    {
        _rotationStrategy = rotationStrategy;
    }
}
