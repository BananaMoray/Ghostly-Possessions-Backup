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
    public MonoBehaviour AttackStrategy;

    private IMovementStrategy _movementStrategy;
    private IAttackStrategy _attackStrategy;



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

        if (AttackStrategy == null)
            Debug.LogError($"Rotation Strategy not implemented for {gameObject.name}");
        else
            _attackStrategy = AttackStrategy as IAttackStrategy;

        //_shootComponent = GetComponent<IShootable>();
        //if (_shootComponent == null)
        //    Debug.LogError($"Shoot Component not implemented for {gameObject.name}");
    }

    private void FixedUpdate()
    {
        if (MovementStrategy == null) return;

        _currentVelocity = _rb.linearVelocity;

        Vector3 targetPos = _player != null ? _player.transform.position : Vector3.zero;

        _movementStrategy.Move(targetPos, _rb, _maxSpeed, _acceleration, _deceleration, _currentVelocity);

        _attackStrategy.Aim(targetPos, _rotationSpeed);
        _attackStrategy.Attack(targetPos);


    }

    public void ChangeMovementStrategy(IMovementStrategy movementStrategy)
    {
        _movementStrategy = movementStrategy;
    }
    public void ChangeRotationStrategy(IAttackStrategy rotationStrategy)
    {
        _attackStrategy = rotationStrategy;
    }
}
