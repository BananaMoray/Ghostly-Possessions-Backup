using TMPro;
using UnityEngine;

public class GameOverState : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tmpText;
    private void Awake()
    {
        _tmpText.text = $"You made it to wave {GameManager.WaveCount}";
    }

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
