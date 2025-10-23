using UnityEngine;

public class BasicPossess : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 60f;
    private PlayerController owner;

    public void OnPossess(PlayerController controller)
    {
        owner = controller;
    }

    public void OnDepossess()
    {
        owner = null;
    }

    public void HandlePossessedUpdate()
    {

    }

    public Transform GetPossessionTransform()
    {
        return transform;
    }
}