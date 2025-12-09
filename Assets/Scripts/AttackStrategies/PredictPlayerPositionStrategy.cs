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

    [SerializeField] 
    private Transform _rayOrigin;

    private void Awake()
    {
        _shootComponent = GetComponent<IShootable>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<PlayerController>();

        if (_rayOrigin == null)
            _rayOrigin = transform;
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



    //private bool HasLineOfSight(Vector3 targetPos)
    //{
    //    Vector3 origin = _rayOrigin.position;
    //    Vector3 direction = (targetPos - origin).normalized;

    //    if (Physics.Raycast(origin, direction, out RaycastHit hit, (direction - origin).sqrMagnitude))
    //    {
    //        return hit.collider.CompareTag("Player");
    //    }

    //    return false;
    //}

    //int sampleCount;

    //Vector3 start;
    //Vector3 end;
    //Vector3 direction;

    //public bool HasLineOfSight(Vector3 enemyPos, Vector3 playerPos, LayerMask obstacleMask)
    //{
    //    start = enemyPos;
    //    end = playerPos;

    //    direction = (end - start);
    //    float distance = direction.magnitude;

    //    direction.Normalize();

    //    sampleCount = Mathf.CeilToInt(distance / _losSampleRadius);

    //    for (int i = 1; i < sampleCount; i++)
    //    {
    //        Vector3 samplePos = start + direction * (i * _losSampleRadius);

    //        Collider[] hits = Physics.OverlapSphere(samplePos, _losSampleRadius, obstacleMask);

    //        foreach (Collider hit in hits)
    //        {
    //            //we simply choose to ignore the player smiles
    //            if (!hit.CompareTag("Player"))
    //                return false;
    //        }
    //    }
    //    return true;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;

    //    // Convert the local coordinate values into world
    //    // coordinates for the matrix transformation.

    //    Gizmos.DrawCube(_predictPos, Vector3.one);

    //    Gizmos.DrawLine(_rayOrigin.position, _predictPos);

    //    for (int i = 1; i < sampleCount; i++)
    //    {
    //        Vector3 gizmoPos= start + direction * (i * _losSampleRadius);
    //        Gizmos.DrawSphere(gizmoPos, _losSampleRadius);
    //    }
    //}


    public void Attack(Vector3 targetPosition)
    {
        _shootComponent.OnRequestAttack(true);
    }
}
