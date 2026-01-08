using UnityEngine;

public class GameOverState : MonoBehaviour
{
    public void SwitchToMainMenu()
    {
        SceneStateManager.Instance.MainMenu();
    }

    public void RestartGame()
    {
        SceneStateManager.Instance.StartGame();
    }

    public void StopApplication()
    {
        Application.Quit();
    }
}
