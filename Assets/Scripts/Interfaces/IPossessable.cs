using UnityEngine;

public interface IPossessable
{

    void OnPossess(PlayerController controller);

    void OnDepossess();

    void HandlePossessedMovement(Vector2 moveInput);
    void HandlePossessedInteract();

    Transform GetPossessionTransform();
}
