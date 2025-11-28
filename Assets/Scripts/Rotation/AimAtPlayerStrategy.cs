using UnityEngine;

public class AimAtPlayerStrategy : MonoBehaviour, IRotationStrategy
{
    public void Rotate(Vector3 targetPosition, float rotationSpeed)
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
}
