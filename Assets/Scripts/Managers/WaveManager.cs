using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    //[SerializeField]
    ////private int _currentWave;
    [SerializeField]
    [Range(0, 10)]
    private int _waveValueBase = 10;
    [SerializeField]
    [Range(1f, 2f)]
    private float _waveValueMultiplier = 1.05f;
    [SerializeField]
    [Range(0, 5f)]
    private float _waveValueDivisor = 3.5f;
    [SerializeField]
    [Range(0, 25)]
    private int _waveOffset = 0;
    [SerializeField]
    private int _maxEnemyAmount = 25;
    [SerializeField]
    private AnimationCurve _curve;

    public int WaveValue;

    public List<EnemyData> enemies = new List<EnemyData>();

    public TextMeshProUGUI UIText;
    public GameObject WaveUI;
    private TextMeshProUGUI _waveText;

    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    [SerializeField]
    [Range(0, 5)]
    private float spawnInterval = 5f;
    private float spawnTimer;

    private int _waveEnemyAmount;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private float _totalWeight = 0;

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
            Instance = this;

        _waveText = WaveUI.GetComponentInChildren<TextMeshProUGUI>();
        WaveUI.SetActive(false);

    }

    void Start()
    {
        StartCoroutine(StartNewWave(3f));
    }

    private void Update()
    {
        UpdateUI();
    }

    void FixedUpdate()
    {

        if (spawnTimer <= 0)
        {
            //spawn an enemy using SpawnManager
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = (GameObject)SpawnManager.Instance.InstantiatePrefab(enemiesToSpawn[0], 35, 3, true);

                //GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity);
                enemiesToSpawn.RemoveAt(0);
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;

            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }


    }

    private void UpdateUI()
    {
        UIText.text = $"Current wave: {GameManager.WaveCount}" +
            $"<br>Enemies to defeat: {_waveEnemyAmount}" +
            $"<br>Current WaveValue: {WaveValue}" +
            $"<br>Total Weight: {_totalWeight}";
    }

    public void GenerateWave()
    {
        //WaveValue = (int)(_waveValueBase + (Mathf.Pow(_waveValueMultiplier, _currentWave - 1)));
        WaveValue = (int)(_waveValueBase
            + (GameManager.WaveCount + _waveOffset / _waveValueDivisor)
            + (Mathf.Pow(_waveValueMultiplier, GameManager.WaveCount + _waveOffset)));
        GenerateEnemies(WaveValue);

        //spawnInterval = waveDuration / enemiesToSpawn.Count;
        _waveEnemyAmount = enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }



    public void GenerateEnemies(int waveValue)
    {
        //int value = waveValue;

        //List<GameObject> generatedEnemies = new List<GameObject>();

        //while (value > 0 || generatedEnemies.Count < _maxEnemyAmount)
        //{
        //    int randEnemyId = Random.Range(0, enemies.Count);
        //    int enemyCost = enemies[randEnemyId].Cost;

        //    if (value - enemyCost >= 0)
        //    {
        //        generatedEnemies.Add(enemies[randEnemyId].EnemyPrefab);
        //        value -= enemyCost;
        //    }
        //    else if (value <= 0)
        //    {
        //        break;
        //    }
        //}
        //enemiesToSpawn.Clear();
        //enemiesToSpawn = generatedEnemies;

        enemiesToSpawn.Clear();

        _totalWeight = 0;

        foreach (EnemyData enemy in enemies)
        {
            _totalWeight += enemy.Weight;
        }

        int spawnCount = Mathf.Clamp(waveValue, 1, _maxEnemyAmount);

        for (int i = 0; i < spawnCount; i++)
        {
            EnemyData selectedEnemy = GetWeightedEnemy();

            if (selectedEnemy == null)
                break;

            enemiesToSpawn.Add(selectedEnemy.EnemyPrefab);
        }
    }

    private EnemyData GetWeightedEnemy()
    {
        float roll = Random.Range(0f, _totalWeight);
        float cumulative = 0f;

        foreach (EnemyData enemy in enemies)
        {
            cumulative += enemy.Weight;
            if (roll <= cumulative)
                return enemy;
        }

        return null; // fallback (should never hit)
    }

    public void DecreaseEnemyCount()
    {
        _waveEnemyAmount -= 1;
        //Debug.Log("-1 enemy");

        if (_waveEnemyAmount <= 0)
        {
            GameManager.WaveCount++;
            //Debug.Log($"Wave {_currentWave} begin");
            StartCoroutine(StartNewWave(3f));
        }

    }

    private IEnumerator StartNewWave(float duration)
    {
        yield return new WaitForSeconds(2f);
        WaveUI.SetActive(true);
        _waveText.text = $"WAVE {GameManager.WaveCount}";

        yield return new WaitForSeconds(duration);
        GenerateWave();
        WaveUI.SetActive(false);
    }

}

[System.Serializable] //otherwise i cant access them in the inspector
public class EnemyData
{
    public GameObject EnemyPrefab;
    [Range(1, 30)]
    [Tooltip("This is the value of the enemy in the store.")]
    public int Cost;
    [Range(.1f, 5)]
    [Tooltip("This is the weight of the enemy on the wheel.")]
    public float Weight = 5;
}