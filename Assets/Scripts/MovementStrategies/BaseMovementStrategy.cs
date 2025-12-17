using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BaseMovementStrategy : MonoBehaviour, IMovementStrategy
{
    [Header("Steering")]
    [SerializeField]
    protected int _directionCount = 16;
    [SerializeField]
    protected float _checkDistance = 10f;

    protected float _losSampleRadius = .7f;

    [SerializeField]
    protected LayerMask _obstacleMask = 7;

    protected Collider _selfCollider;

    protected Vector3 _playerPos;

    protected virtual void Awake()
    {
        _selfCollider = GetComponent<Collider>();
    }

    public virtual Vector3 GetDesiredDirection(Vector3 enemyPos, Vector3 targetPos, EnemyIntention intention)
    {
        _playerPos = targetPos;

        Vector3 intentDir = GetIntentionDirection(enemyPos, targetPos, intention);

        return GetBestSteeredDirection(enemyPos, intentDir);
    }


    protected virtual Vector3 GetIntentionDirection(Vector3 enemyPos, Vector3 targetPos, EnemyIntention intention)
    {
        return (targetPos - enemyPos).normalized;
    }

    protected Vector3 GetBestSteeredDirection(Vector3 origin, Vector3 preferredDir)
    {
        float bestScore = float.NegativeInfinity;
        Vector3 bestDir = Vector3.zero;

        for (int i = 0; i < _directionCount; i++)
        {
            float angle = (360f / _directionCount) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            if (!HasLineOfSightInDirection(origin, dir))
                continue;

            float dot = Vector3.Dot(dir.normalized, preferredDir);
            float score = dot;

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        return bestDir.normalized;
    }

    public bool HasLineOfSightInDirection(Vector3 origin, Vector3 direction)
    {
        direction.Normalize();
        int samples = Mathf.CeilToInt(_checkDistance / _losSampleRadius);

        for (int i = 1; i <= samples; i++)
        {
            Vector3 samplePos = origin + direction * (i * _losSampleRadius);

            Collider[] hits = Physics.OverlapSphere(samplePos, _losSampleRadius);

            foreach (Collider hit in hits)
            {
                //ignore self idiot
                if (hit == _selfCollider)
                    continue;

                //Debug.Log($"Collided with {hit.gameObject.name}");

                return false;
            }
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 preferred = (_playerPos - origin).normalized;

        int count = 16;
        float maxLength = 5f;

        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            float dot = Vector3.Dot(dir, preferred);
            float length = Mathf.Lerp(1f, maxLength, (dot + 1f) * 0.5f);

            bool clear = true;

            clear = HasLineOfSightInDirection(origin, dir);

            Gizmos.color = clear ? Color.green : Color.red;
            Gizmos.DrawLine(origin, origin + dir * length);

            //for (int j = 1; j <= 6; j++)
            //{
            //    Vector3 samplePos = origin + dir * (j * _losSampleRadius);

            //    Gizmos.DrawSphere(samplePos, _losSampleRadius);
            //}
        }


    }
}
