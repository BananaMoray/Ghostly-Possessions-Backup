using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicPossess : MonoBehaviour, IPossessable
{
    private PlayerController owner;

    [Header("Movement Variables")]
    [SerializeField] private float _acceleration = 20f;
    [SerializeField] private float _deceleration = 8f;
    [SerializeField] private float _maxSpeed = 3f;
    private Vector3 _currentVelocity = Vector3.zero;
    private Vector2 _moveInput;

    private MeshRenderer _renderer;
    private Material _normalMat;

    
        

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _normalMat = _renderer.material;
        
    }

    public void OnPossess(PlayerController controller)
    {
        owner = controller;
        _renderer.material = controller.GetComponent<Renderer>().material;
    }

    public void OnDepossess()
    {
        owner = null;
        _renderer.material = _normalMat;
    }

    public Transform GetPossessionTransform()
    {
        return transform;
    }

    public void HandlePossessedMovement(Vector2 moveInput)
    {
        //copy pasted code from 
        Vector3 inputVelocity = new Vector3(moveInput.x, moveInput.y, 0) * _maxSpeed;

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

        transform.position += _currentVelocity * Time.deltaTime;
    }

    public void HandlePossessedInteract(bool interactInput)
    {
        throw new System.NotImplementedException();
    }
}