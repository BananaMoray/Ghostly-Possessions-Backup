using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AimAtPlayerStrategy : MonoBehaviour, IAttackStrategy
{
    private IShootable _shootComponent;
    [SerializeField]
    private float _validShootDistance = 12f;

    private float _losSampleRadius = 0.3f;
    [SerializeField]
    private LayerMask _losMask;
    private void Awake()
    {
        _shootComponent = gameObject.GetComponent<IShootable>();
    }

    private bool IsPlayerInRange(Vector3 targetPos)
    {
        if (targetPos == Vector3.zero) return false;

        return ((targetPos - transform.position).sqrMagnitude <= _validShootDistance * _validShootDistance);
    }

    public void Aim(Vector3 targetPosition, float rotationSpeed)
    {
        //same as move
        if (targetPosition == Vector3.zero) return;

        Vector3 lookDirection = targetPosition - transform.position;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }


    public void Attack(Vector3 targetPosition)
    {
        _shootComponent.OnRequestAttack(true);

        //if (HasLineOfSight(transform.position, targetPosition, _losMask))
        //{
        //    _shootComponent.OnRequestAttack(true);
        //}
        //else
        //{
        //    _shootComponent.OnRequestAttack(false);
        //}
    }

    public bool HasLineOfSight(Vector3 enemyPos, Vector3 playerPos, LayerMask obstacleMask)
    {
        Vector3 start = enemyPos;
        Vector3 end = playerPos;

        Vector3 direction = (end - start);
        float distance = direction.magnitude;

        direction.Normalize();

        int sampleCount = Mathf.CeilToInt(distance / _losSampleRadius);

        for (int i = 1; i < sampleCount; i++)
        {
            Vector3 samplePos = start + direction * (i * _losSampleRadius);

            Collider[] hits = Physics.OverlapSphere(samplePos, _losSampleRadius, obstacleMask);

            foreach (Collider hit in hits)
            {
                //we simply choose to ignore the player smiles
                if (!hit.CompareTag("Player"))
                    return false;
            }
        }
        return true;
    }
}
