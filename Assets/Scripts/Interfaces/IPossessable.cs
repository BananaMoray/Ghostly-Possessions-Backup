using UnityEngine;

public interface IPossessable
{

    void OnPossess(PlayerController controller);

    void OnDepossess();

    void HandlePossessedMovement(Vector2 moveInput);
    void HandlePossessedRotation(Vector2 lookInput);
    void HandlePossessedAttack(bool attackInput);

    Transform GetPossessionTransform();
}
