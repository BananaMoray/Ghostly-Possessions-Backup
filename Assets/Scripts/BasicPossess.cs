using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicPossess : MonoBehaviour, IPossessable
{
    private PlayerController owner;

    [Header("Movement Variables")]
    public bool SpaceShipMove = false;
    public bool SpaceShipRotate = false;
    [SerializeField] 
    private float _acceleration = 20f;
    [SerializeField] 
    private float _deceleration = 8f;
    [SerializeField] 
    private float _maxSpeed = 3f;
    [SerializeField]
    private float _sideMoveMultiplier = 0.3f;
    [SerializeField]
    private float _backMoveMultiplier = 0.2f;
    [SerializeField]
    private float _rotationSpeed = 360f;
    private Vector3 _currentVelocity = Vector3.zero;

    private MeshRenderer _renderer;
    private Material _normalMat;
    [SerializeField]
    private Material _possessMat;

    private Rigidbody _rb;

    private IShootable _shootLogic;



    [Header("Thrusters")]
    public ParticleSystem[] _thrusterParticleSystems;
    public List<ParticleSystem.EmissionModule> _thrusterEmission;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _renderer = GetComponent<MeshRenderer>();
        _normalMat = _renderer.material;

        _shootLogic = GetComponent<IShootable>();

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
    }

    public void OnPossess(PlayerController controller)
    {
        owner = controller;
        _renderer.material = _possessMat;
    }

    public void OnDepossess()
    {
        owner = null;
        _renderer.material = _normalMat;
        if (_rb != null) _rb.linearVelocity = Vector2.zero;

    }

    public Transform GetPossessionTransform()
    {
        return transform;
    }

    public void HandlePossessedMovement(Vector2 moveInput)
    {
        if (_rb == null) return;

        Vector3 inputVelocity = Vector3.zero;

        if (!SpaceShipMove)
        {
            //Absolute-Movement
            inputVelocity = new Vector3(moveInput.x, 0, moveInput.y) * _maxSpeed;
        }
        else
        {
            //Relative-Movement
            float x = moveInput.x * _sideMoveMultiplier;
            float z = moveInput.y > 0 ? moveInput.y : moveInput.y * _backMoveMultiplier;

            inputVelocity =
                (transform.right * x * (_maxSpeed / 2)) +
                (transform.forward * z * _maxSpeed);
        }

        _currentVelocity = Vector3.MoveTowards(
            _currentVelocity,
            inputVelocity,
            _acceleration * Time.deltaTime
        );

        if (moveInput.magnitude < 0.01f)
        {
            _currentVelocity = Vector3.MoveTowards(
                _currentVelocity,
                Vector3.zero,
                _deceleration * Time.deltaTime
            );
        }


        _rb.linearVelocity = _currentVelocity;



        if (_thrusterParticleSystems.Length != 0)
        {
            bool thrusting = moveInput.y > 0.1f;

            foreach (var ps in _thrusterParticleSystems)
            {
                var em = ps.emission;
                em.enabled = thrusting;
            }
        }
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
     
    public void HandlePossessedRotation(Vector2 lookInput)
    {

        if(SpaceShipRotate)
            transform.Rotate(0.0f, lookInput.x * _rotationSpeed * Time.deltaTime, 0.0f, Space.Self);
        else
        {
            
            Vector3 direction = (_cameraUp * lookInput.y + _cameraRight * lookInput.x);

            if (direction.sqrMagnitude < 0.01f)
                direction = (_cameraUp * lookInput.y + _cameraRight * lookInput.x);

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

    }


    public virtual void HandlePossessedAttack(bool attackInput)
    {
        if (_shootLogic != null)
        {
            _shootLogic.OnRequestAttack(attackInput);
        }
    }
}