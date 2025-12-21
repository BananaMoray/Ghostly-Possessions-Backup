using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SceneManager _sceneManager;

    public GameObject[] waves;

    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private float _spawnRadius = 40f;
    [SerializeField]
    private float _checkRadius = 2f;
    [SerializeField]
    private int _spawnCount = 30;

    private void Awake()
    {
        SpawnManager.Instance.SpawnPrefab(_asteroidPrefab, _spawnRadius, _checkRadius, _spawnCount, false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
