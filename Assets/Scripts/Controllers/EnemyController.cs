using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum EnemyIntention
{
    //various "intentions" the ai could have in order to make its next decision
    Idle,
    Pursuing,
    Attacking,
    Retreating,
    Evading
}

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

    private IHealth _healthComponent;

    [Header("Strategies")]
    public MonoBehaviour MovementStrategy;
    public MonoBehaviour AttackStrategy;

    private IMovementStrategy _movementStrategy;
    private IAttackStrategy _attackStrategy;

    [Header("Intentions and Behaviour")]
    public EnemyIntention CurrentIntention = EnemyIntention.Pursuing;
    [SerializeField]
    private float _attackRange = 12f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _cowardice = 0.1f;
    [SerializeField]
    [Range(0f, 5f)]
    private float _retreatTimer = 2.5f;
    [SerializeField]
    private float _losCoolDown = 2f;
    private float _lostimer;

    private float _losSampleRadius = 0.3f;
    [SerializeField]
    private LayerMask _losMask;

    private Collider _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb != null)
            _rb.freezeRotation = true;

        _collider = GetComponent<Collider>();

        _player = GameObject.FindGameObjectWithTag("Player");

        if (_healthComponent == null)
            _healthComponent = GetComponent<IHealth>();

        if (MovementStrategy == null)
            Debug.LogError($"Movement Strategy not implemented for {gameObject.name}");
        else
            _movementStrategy = MovementStrategy as IMovementStrategy;

        if (AttackStrategy == null)
            Debug.LogError($"Attack Strategy not implemented for {gameObject.name}");
        else
            _attackStrategy = AttackStrategy as IAttackStrategy;

        //_shootComponent = GetComponent<IShootable>();
        //if (_shootComponent == null)
        //    Debug.LogError($"Shoot Component not implemented for {gameObject.name}");
    }

    private void FixedUpdate()
    {
        UpdateIntention();

        //current velocity needs constant refreshing
        _currentVelocity = _rb.linearVelocity;
        Vector3 targetPos = _player != null ? _player.transform.position : Vector3.zero;

        HandleMovementIntention(targetPos);

        HandleAttackIntention(targetPos);

    }

    private void HandleAttackIntention(Vector3 targetPos)
    {
        //enemy always aims
        _attackStrategy.Aim(targetPos, _rotationSpeed);

        //only attack if the intention is attacking
        if (CurrentIntention == EnemyIntention.Attacking)
            _attackStrategy.Attack(targetPos);
    }

    private void HandleMovementIntention(Vector3 targetPos)
    {
        //case switches to differentiate between 

        switch (CurrentIntention)
        {
            //if pursuing, perfom previous established movement  
            case EnemyIntention.Pursuing:
                _movementStrategy.Move(targetPos, _rb, _maxSpeed, _acceleration, _deceleration, _currentVelocity);
                break;

            //when attacking, stay in position, dont move
            case EnemyIntention.Attacking:
                _movementStrategy.Move(transform.position, _rb, _maxSpeed, _acceleration, _deceleration, _currentVelocity);
                break;

            //calculate the inverse of the player position, then retreat there
            case EnemyIntention.Retreating:
                Vector3 retreatPos = transform.position - (targetPos - transform.position).normalized * 6f;
                _movementStrategy?.Move(retreatPos, _rb, _maxSpeed, _acceleration, _deceleration, _currentVelocity);
                break;

            //move perpendicular to the current position to potentially evade attacks
            case EnemyIntention.Evading:
                //cross product yuippei
                Vector3 perpendicularPos = Vector3.Cross((targetPos - transform.position).normalized, transform.up);
                _movementStrategy.Move(targetPos * 0.2f + perpendicularPos * 10f, _rb, _maxSpeed, _acceleration, _deceleration, _currentVelocity);
                break;
        }

        Debug.Log(CurrentIntention);
    }

    private void UpdateIntention()
    {
        bool hasLineOfSight = HasLineOfSight(transform.position, _player.transform.position, _losMask);

        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);



        if (CurrentIntention == EnemyIntention.Retreating)
        {
            _retreatTimer -= Time.deltaTime;

            if (_retreatTimer <= 0 || distanceToPlayer > _attackRange * 1.5f)
            {
                CurrentIntention = EnemyIntention.Pursuing;
            }
            return;
        }

        if (_healthComponent.GetCurrentHealthPercent() < _cowardice && _retreatTimer > 0)
        {
            CurrentIntention = EnemyIntention.Retreating;
            return;
        }

        //if the line of sight ot the player is established and the player is within range, attack!!!
        //attack range in the controller allows me to get rid of ti in the shoot base class
        if (distanceToPlayer < _attackRange)
        {
            if (hasLineOfSight)
            {
                CurrentIntention = EnemyIntention.Attacking;
                Debug.Log($"Attacking Activated");
                _lostimer = 0;
            }
            else
            {
                CurrentIntention = EnemyIntention.Evading;
                _lostimer += Time.deltaTime;

                if (_lostimer >= _losCoolDown)
                {
                    CurrentIntention = EnemyIntention.Pursuing;
                }
            }
        }
        else
            CurrentIntention = EnemyIntention.Pursuing;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_origin, _direction);
    }

    Vector3 _origin = Vector3.zero;
    Vector3 _direction = Vector3.zero;

    public bool HasLineOfSight(Vector3 enemyPos, Vector3 playerPos, LayerMask obstacleMask)
    {
        Vector3 start = enemyPos;
        Vector3 end = playerPos;

        Vector3 direction = (end - start);
        float distance = direction.magnitude;

        direction.Normalize();

        int sampleCount = Mathf.CeilToInt(distance / _losSampleRadius);

        for (int i = 1; i < sampleCount; i++)
        {
            Vector3 samplePos = start + direction * (i * _losSampleRadius);

            Collider[] hits = Physics.OverlapSphere(samplePos, _losSampleRadius, obstacleMask);

            foreach (Collider hit in hits)
            {

                //if you hit yourself, ignore
                if (hit == _collider)
                    continue;


                ////if the object is not the player, set false
                //if (hit.CompareTag("Player"))
                //{
                //    Debug.Log($"Hit playerrr");
                //    return true;
                //}



                //Debug.Log($"Hit {hit.name} at collider {hit}");
                return false;
            }
        }
        return true;
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
