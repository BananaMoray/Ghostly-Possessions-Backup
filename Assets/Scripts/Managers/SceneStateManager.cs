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
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R) && GameManager.DebugMode)
    //    {
    //        LoadScene(SceneManager.GetActiveScene().name);
    //    }

    //    _loadingTimer += Time.deltaTime;


    //}

    public void LoadScene(string sceneName)
    {
        SoundMixerManager.Instance.SetMasterPitch(1);
        SceneManager.LoadScene(sceneName);

    }

    public void ResetScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
        GameManager.PossessableShipsDictionary.Clear();
        GameManager.WaveCount = 1;
    }

    public async void StartGame()
    {

        GameManager.PossessableShipsDictionary.Clear();
        GameManager.WaveCount = 1;

        Time.timeScale = 1.0f;

        var scene = SceneManager.LoadSceneAsync(_playScene);
        scene.allowSceneActivation = false;

        if (_loadingCanvas != null)
            _loadingCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);

        } while (scene.progress < 0.9f);

        await Task.Delay(3500);

        scene.allowSceneActivation = true;

        await Task.Delay(200);

        if (_loadingCanvas != null)
            _loadingCanvas.SetActive(false);


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
