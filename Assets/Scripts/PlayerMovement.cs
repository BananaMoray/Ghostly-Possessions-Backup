using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 8f;
    [SerializeField] private float _maxSpeed = 3f;
    private Vector3 _currentVelocity = Vector3.zero;

    [Header("Possession Movement")]
    [SerializeField] private AnimationCurve _easeInOut;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _possessingDistanceCheck = 0.5f;

    [HideInInspector] public Vector2 _movementInput = Vector2.zero;
    [HideInInspector] public Vector2 _lookInput = Vector2.zero;

    private float _currentProgress;

    public void HandleMovement(bool isPossessing, bool isPossessionInProgress, GameObject possessionObject)
    {
        if (isPossessing)
        {
            HandlePossessionMovement(isPossessionInProgress, possessionObject);
        }
        else
        {
            HandleNormalMovement();
        }
    }

    private void HandleNormalMovement()
    {
        Vector3 inputVelocity = new Vector3(_movementInput.x, _movementInput.y, 0) * _maxSpeed;

        _currentVelocity = Vector3.MoveTowards(
            _currentVelocity,
            inputVelocity,
            _acceleration * Time.deltaTime
        );

        if (_movementInput.magnitude < 0.01f)
        {
            _currentVelocity = Vector3.MoveTowards(
                _currentVelocity,
                Vector3.zero,
                _deceleration * Time.deltaTime
            );
        }

        transform.position += _currentVelocity * Time.deltaTime;
    }

    private void HandlePossessionMovement(bool isPossessionInProgress, GameObject possessionObject)
    {
        if (possessionObject == null) return;

        float s = _currentProgress / _duration;
        Vector3 currentPos = transform.position;
        Vector3 targetPos = possessionObject.transform.position;

        transform.position = Vector3.MoveTowards(currentPos, targetPos, _easeInOut.Evaluate(s));

        if (Vector3.Distance(transform.position, targetPos) < _possessingDistanceCheck)
        {
            if (isPossessionInProgress && TryGetComponent<PlayerController>(out var controller))
            {
                controller.IsPossessionInProgress = false;
                controller.PossessObject(possessionObject);
            }
        }
        else
        {
            //Debug.Log("Possession in progress");
            _currentProgress += Time.deltaTime;
        }
    }

    public void ResetProgress()
    {
        _currentProgress = 0f;
    }
}
