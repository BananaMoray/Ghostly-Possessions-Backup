using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] 
    private float _acceleration = 10f;
    [SerializeField] 
    private float _deceleration = 8f;
    [SerializeField] 
    private float _maxSpeed = 3f;
    private Vector3 _currentVelocity = Vector3.zero;

    [Header("Possession Movement")]
    [SerializeField] 
    private AnimationCurve _easeInOut;
    [SerializeField] 
    private float _duration = 1f;
    [SerializeField] 
    private float _possessingDistanceCheck = 0.5f;

    [HideInInspector] 
    public Vector2 MovementInput = Vector2.zero;
    [HideInInspector]
    public Vector2 LookInput = Vector2.zero;

    private float _currentPossessionProgress;

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
        Vector3 inputVelocity = new Vector3(MovementInput.x, 0, MovementInput.y) * _maxSpeed;

        _currentVelocity = Vector3.MoveTowards(
            _currentVelocity,
            inputVelocity,
            _acceleration * Time.deltaTime
        );

        if (MovementInput.magnitude < 0.01f)
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

        _currentPossessionProgress += Time.deltaTime;

        float time = Mathf.Clamp01(_currentPossessionProgress / _duration);

        float easedTime = _easeInOut.Evaluate(time);

        Vector3 startPos = PlayerController.StartPossessionPosition;
        Vector3 targetPos = possessionObject.transform.position;
        transform.position = Vector3.Lerp(startPos, targetPos, easedTime);

        if (time >= 1f || Vector3.Distance(transform.position, targetPos) < _possessingDistanceCheck)
        {
            if (isPossessionInProgress && TryGetComponent<PlayerController>(out var controller))
            {
                controller.PossessObject(possessionObject);
            }
        }
    }

    public void ResetProgress()
    {
        _currentPossessionProgress = 0f;
    }
}
