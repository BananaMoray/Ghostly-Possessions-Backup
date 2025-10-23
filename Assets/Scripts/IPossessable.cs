using UnityEngine;

public interface IPossessable
{

    void OnPossess(PlayerController controller);

    void OnDepossess();

    void HandlePossessedUpdate();

    Transform GetPossessionTransform();
}
