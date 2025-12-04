using System;
using UnityEngine;

public class AimAtPlayerStrategy : MonoBehaviour, IAttackStrategy
{
    private IShootable _shootComponent;
    [SerializeField]
    private float _validShootDistance = 12f;


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
        _shootComponent.OnRequestAttack(IsPlayerInRange(targetPosition));
    }
}
