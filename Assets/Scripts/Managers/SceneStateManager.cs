using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    private string _playScene = "PlayScene";
    private string _mainMenuScene = "MainMenu";
    private string _gameOverScene = "GameOver";

    [SerializeField]
    private GameObject _loadingCanvas;

    private float _loadingTimer;

    private float _progressTimer;

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        _loadingTimer += Time.deltaTime;


    }

    public async void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        //_loadingTimer = 0;
        _progressTimer = 0;

        _loadingCanvas.SetActive(true);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        _loadingCanvas.SetActive(true);

    }

    public void ResetScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
        GameManager.PossessableShipsDictionary.Clear();
        GameManager.WaveCount = 1;
    }
    
    public void StartGame()
    {
        LoadScene(_playScene);
        GameManager.PossessableShipsDictionary.Clear();
        GameManager.WaveCount = 1;
    }

    public void MainMenu()
    {
        LoadScene(_mainMenuScene);
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
        LoadScene(_gameOverScene);
    }

    private IEnumerator EndGameRoutine()
    {
        yield return new WaitForSecondsRealtime(4);
        EnterGameOver();
    }

}
