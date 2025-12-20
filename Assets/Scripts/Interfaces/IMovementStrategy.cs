using UnityEngine;

public interface IMovementStrategy
{


    Vector3 GetDesiredDirection(Vector3 enemyPos, Vector3 targetPos, EnemyIntention intention);
}
