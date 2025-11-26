using System;
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
    private float _rotationSpeed = 360f;
    private Vector3 _currentVelocity = Vector3.zero;

    private MeshRenderer _renderer;
    private Material _normalMat;
    [SerializeField]
    private Material _possessMat;

    private Rigidbody _rb;

    private IShootable _shootLogic;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _renderer = GetComponent<MeshRenderer>();
        _normalMat = _renderer.material;

        _shootLogic = GetComponent<IShootable>();

        _mainCamera = Camera.main;
        CalculateCameraDirections();
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
        //copy pasted code from player
        Vector3 inputVelocity = Vector3.zero;

        if (!SpaceShipMove)
        {
            //normal movement
            inputVelocity = new Vector3(moveInput.x, 0, moveInput.y) * _maxSpeed;
        }
        else 
        {
            //rotation Movement
            //inputVelocity = transform.forward * moveInput.y * _maxSpeed;
            inputVelocity = transform.right * moveInput.x * (_maxSpeed / 2) + transform.forward * moveInput.y * _maxSpeed; 

            //HandleRotation(moveInput.x);
        }

        _currentVelocity = Vector3.MoveTowards(_currentVelocity, inputVelocity, _acceleration * Time.deltaTime);

        if (moveInput.magnitude < 0.01f)
        {
            _currentVelocity = Vector3.MoveTowards(
                _currentVelocity,
                Vector3.zero,
                _deceleration * Time.deltaTime
            );
        }

        //_rb.AddForce(_currentVelocity * PlayerController.PlayerStrength);
        transform.position += _currentVelocity * Time.deltaTime;

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


    public virtual void HandlePossessedInteract()
    {
        if (_shootLogic != null)
        {
            _shootLogic.OnShoot();
        }
    }
}