using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class SpaceshipController : MonoBehaviour, IPossessable
{
    public static event EventHandler<PossessEventArgs> OnSpaceShipDeath;

    private PlayerController owner;

    [Header("Movement Settings")]
    public bool UseRelativeMovement = false;
    public bool UseRelativeRotation = false;
    [Header("Accelleration and Deceleration")]
    [Tooltip("Increases Speed at which Velocity is being gained.")]
    [SerializeField]
    private float _acceleration = 15f;
    [Tooltip("Increases Speed at which Velocity is being lost.")]
    [SerializeField]
    private float _deceleration = 8f;
    [Header("Speed Variables")]
    [SerializeField]
    private float _maxSpeed = 6f;
    [Tooltip("Maximum Speed at which the player moves when boosting.")]
    [SerializeField]
    private float _boostSpeed = 10f;
    [Tooltip("Multiplier by which the HP is drained while boosting.")]
    [SerializeField]
    private float _BoostHPDrainMultiploer = 3f;
    [SerializeField]
    private float _rotationSpeed = 360f;
    public Vector3 CurrentVelocity = Vector3.zero;

    private MeshRenderer _renderer;
    private Material _normalMat;
    [SerializeField]
    private Material _possessMat;

    private Rigidbody _rb;

    private IShootable _shootComponent;

    private IHealth _healthComponent;

    public Vector2 MoveDirection;

    [Header("Thrusters")]
    public ParticleSystem[] _thrusterParticleSystems;
    public List<ParticleSystem.EmissionModule> _thrusterEmission;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _renderer = GetComponent<MeshRenderer>();
        _normalMat = _renderer.material;

        _shootComponent = GetComponent<IShootable>();
        _healthComponent = GetComponent<IHealth>();

        _mainCamera = Camera.main;
        CalculateCameraDirections();

        _thrusterParticleSystems = GetComponentsInChildren<ParticleSystem>(true);

        foreach (ParticleSystem ps in _thrusterParticleSystems)
        {
            var em = ps.emission;
            em.enabled = false;
        }

        if (_rb != null)
        {
            _rb.freezeRotation = true;
        }

        if (_healthComponent is HPLogic hpLogic)
            hpLogic.OnDied += HandleDeath;
    }

    public void OnStartPossess(PlayerController controller)
    {
        owner = controller;
        _renderer.material = _possessMat;
        gameObject.layer = 3;

        _healthComponent.HealthDrainEnabled = true;
        _healthComponent.SetHPBarActive(true);
        _healthComponent.SetOriginalColour();
    }

    public void OnStopPossess()
    {
        owner = null;
        _renderer.material = _normalMat;
        if (_rb != null) _rb.linearVelocity = Vector2.zero;
        gameObject.layer = 0;

        EnableThrusters(false);
        _healthComponent.HealthDrainEnabled = false;
        _healthComponent.SetHPBarActive(false);
        _healthComponent.SetOriginalColour();
    }

    private void HandleDeath()
    {
        OnSpaceShipDeath?.Invoke(this, new PossessEventArgs(owner.gameObject));

        if (owner != null)
            owner.UnpossessObject();
    }

    public Transform GetPossessionTransform()
    {
        return transform;
    }

    public void HandlePossessedInput(Vector2 moveInput, Vector2 lookInput)
    {
        HandleMovement(moveInput);

        HandleRotation(moveInput, lookInput);

        _rb.linearVelocity = CurrentVelocity;

        EnableThrusters(_isBoosting);
    }

    private void HandleMovement(Vector2 moveInput)
    {
        if (_rb == null) return;

        Vector3 inputVelocity = Vector3.zero;

        //Absolute-Movement
        inputVelocity = new Vector3(moveInput.x, 0, moveInput.y) * _maxSpeed;

        if (_isBoosting)
        {
            float x = moveInput.x;
            float z = _isBoosting == true ? 1 : 0;

            inputVelocity = (transform.right * x) + (transform.forward * z * _boostSpeed);

        }

        CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, inputVelocity, _acceleration * Time.deltaTime);

        if (moveInput.magnitude < 0.01f)
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, _deceleration * Time.deltaTime);
        }
    }

    private void EnableThrusters(bool b)
    {
        if (_healthComponent != null)
            (_healthComponent as HPLogic).HpDrainRateInSeconds = b ? 1 / _BoostHPDrainMultiploer : 1;

        if (_thrusterParticleSystems.Length != 0)
        {
            foreach (var ps in _thrusterParticleSystems)
            {
                var em = ps.emission;
                em.enabled = b;
            }
        }
    }

    private bool _isBoosting;

    public virtual void HandlePossessedBoost(bool boostInput)
    {
        _isBoosting = boostInput;
    }

    //camera stuff
    private Camera _mainCamera;
    private Vector3 _cameraUp;
    private Vector3 _cameraRight;

    private void CalculateCameraDirections()
    {
        if (_mainCamera == null) return;

        _cameraUp = _mainCamera.transform.up;
        _cameraRight = _mainCamera.transform.right;
        _cameraUp.y = 0;
        _cameraRight.y = 0;
        _cameraUp.Normalize();
        _cameraRight.Normalize();
    }

    Vector3 _lookDirection = Vector3.zero;

    public void HandleRotation(Vector2 moveInput, Vector2 lookInput)
    {

        if (!_isBoosting)
        {
            if (lookInput.sqrMagnitude > 0.01f)
                _lookDirection = (_cameraUp * lookInput.y + _cameraRight * lookInput.x);
        }
        else
        {
            if (moveInput.sqrMagnitude > 0.01f)
                _lookDirection = (_cameraUp * moveInput.y + _cameraRight * moveInput.x);
        }

        if (_lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }


    public virtual void HandlePossessedAttack(bool attackInput)
    {
        if (_shootComponent != null)
        {
            _shootComponent.OnRequestAttack(attackInput);
        }
    }
}