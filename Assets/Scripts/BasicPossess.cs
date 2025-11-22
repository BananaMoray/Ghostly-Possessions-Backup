using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicPossess : MonoBehaviour, IPossessable
{
    private PlayerController owner;

    [Header("Movement Variables")]
    [SerializeField]
    private bool MovementRotate = false;
    [SerializeField] 
    private float _acceleration = 20f;
    [SerializeField] 
    private float _deceleration = 8f;
    [SerializeField] 
    private float _maxSpeed = 3f;
    [SerializeField]
    private float _rotationSpeed = 360f;
    private Vector3 _currentVelocity = Vector3.zero;
    private Vector2 _moveInput;

    private MeshRenderer _renderer;
    private Material _normalMat;

    private Rigidbody _rb;

    private IShootable _shootLogic;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _renderer = GetComponent<MeshRenderer>();
        _normalMat = _renderer.material;

        _shootLogic = GetComponent<IShootable>();
    }

    public void OnPossess(PlayerController controller)
    {
        owner = controller;
        _renderer.material = owner.PossessMat;
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

        if (!MovementRotate)
        {
            //normal movement
            inputVelocity = new Vector3(moveInput.x, 0, moveInput.y) * _maxSpeed;
        }
        else 
        {
            //rotation Movement
            inputVelocity = transform.forward * moveInput.y * _maxSpeed;

            HandleRotation(moveInput.x);
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

    private void HandleRotation(float x)
    {

        transform.Rotate(0.0f, x * _rotationSpeed * Time.deltaTime, 0.0f, Space.Self);

    }

    public virtual void HandlePossessedInteract()
    {
        if (_shootLogic != null)
        {
            _shootLogic.OnShoot();
        }
    }
}