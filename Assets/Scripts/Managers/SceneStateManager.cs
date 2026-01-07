using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    private string _playScene = "PlayScene";
    private string _mainMenuScene = "MainMenu";
    private string _gameOverScene = "GameOver";

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
            Instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScene()
    {
        SwitchScene(SceneManager.GetActiveScene().name);
        GameManager.PossessableShipsDictionary.Clear();
        GameManager.WaveCount = 1;
    }
    
    public void StartGame()
    {
        SwitchScene(_playScene);
        GameManager.PossessableShipsDictionary.Clear();
        GameManager.WaveCount = 1;
    }

    public void MainMenu()
    {
        SwitchScene(_mainMenuScene);
    }

    public void EndGame()
    {
        StartCoroutine(EndGameRoutine());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void EnterGameOver()
    {
        SwitchScene(_gameOverScene);
    }

    private IEnumerator EndGameRoutine()
    {
        yield return new WaitForSecondsRealtime(4);
        EnterGameOver();
    }
}
