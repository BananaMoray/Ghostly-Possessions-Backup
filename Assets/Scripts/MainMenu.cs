using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneStateManager.Instance.StartGame();
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
