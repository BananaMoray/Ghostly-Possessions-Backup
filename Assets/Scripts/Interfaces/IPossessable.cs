using UnityEngine;

public interface IPossessable
{

    void OnStartPossess(PlayerController controller);

    void OnStopPossess();

    void HandlePossessedInput(Vector2 moveInput, Vector2 lookInput);
    void HandlePossessedLateUpdate();
    void HandlePossessedAttack(bool attackInput);
    void HandlePossessedBoost(bool attackInput);

    Transform GetPossessionTransform();

    public Vector3 CurrentVelocity {  get; set; }
}
