using UnityEngine;

public class FollowPlayerStrategy : BaseMovementStrategy
{
    [SerializeField] private float _minDistance = 5f;

    public FollowPlayerStrategy() { }

    protected override Vector3 GetIntentionDirection(Vector3 enemyPos, Vector3 targetPos, EnemyIntention intention)
    {

        Vector3 toPlayer = targetPos - enemyPos;

        if (toPlayer.sqrMagnitude < _minDistance * _minDistance)
            return Vector3.zero;

        return toPlayer.normalized;
    }
}
