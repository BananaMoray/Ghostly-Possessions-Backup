using UnityEditor;
using UnityEngine;

public class PredictPlayerPositionStrategy : MonoBehaviour, IAttackStrategy
{
    private IShootable _shootComponent;

    [Header("Prediction Settings")]
    [SerializeField]
    private float _validShootDistance = 12f;
    [Tooltip("Higher value means better prediction abilities. 0 has no tracking. 1 is perfect tracking. 2 overshoots.")]
    [Range(0f, 2f)]
    [SerializeField]
    private float _aimMoreMultiplier = 1.0f;
    [SerializeField]
    private float _projectileSpeed = 30f;

    private PlayerController _playerMovement;

    private void Awake()
    {
        _shootComponent = GetComponent<IShootable>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<PlayerController>();
    }

    private bool IsPlayerInRange(Vector3 targetPos)
    {
        if (targetPos == Vector3.zero) return false;
        return (targetPos - transform.position).sqrMagnitude <= _validShootDistance * _validShootDistance;
    }

    private Vector3 _predictPos;


    public void Aim(Vector3 targetPosition, float rotationSpeed)
    {
        if (targetPosition == Vector3.zero) return;

        Vector3 playerVelocity = _playerMovement != null ? _playerMovement.CurrentVelocity : Vector3.zero;

        Vector3 predictedPosition = GetPredictedPosition(targetPosition, playerVelocity);

        _predictPos = predictedPosition;

        Vector3 lookDirection = predictedPosition - transform.position;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
    private Vector3 GetPredictedPosition(Vector3 playerPosition, Vector3 playerVelocity)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        float timeToHit = distanceToPlayer / _projectileSpeed;

        Vector3 predictedPos = playerPosition + playerVelocity * timeToHit * _aimMoreMultiplier;

        return predictedPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.

        Gizmos.DrawCube(_predictPos, Vector3.one);
    }

    public void Attack(Vector3 targetPosition)
    {
        _shootComponent.OnRequestAttack(IsPlayerInRange(targetPosition));
    }
}
