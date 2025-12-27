using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private int _currentWave;

    [SerializeField]
    private float _waveValueMultiplier = 1.05f;
    [SerializeField]
    private float _waveValueDivisor = 3.5f;
    [SerializeField]
    private int _waveValueBase = 10;
    [SerializeField]
    private int _maxEnemyAmount = 25;
    [SerializeField]
    private AnimationCurve _curve;

    public int WaveValue;

    public List<Enemy> enemies = new List<Enemy>();


    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    private int _waveEnemyAmount;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        GenerateWave();
    }

    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            //spawn an enemy using SpawnManager
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = (GameObject)SpawnManager.Instance.InstantiatePrefab(enemiesToSpawn[0], 40, 3, true);

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

        if (waveTimer <= 0)
        {
            _currentWave++;
            Debug.Log($"Wave {_currentWave} begin");
            GenerateWave();
        }
    }

    public void GenerateWave()
    {
        //WaveValue = (int)(_waveValueBase + (Mathf.Pow(_waveValueMultiplier, _currentWave - 1)));
        WaveValue = (int)(_waveValueBase + (_currentWave / _waveValueDivisor) + (Mathf.Pow(_waveValueMultiplier, _currentWave - 1)));
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;
        _waveEnemyAmount = enemiesToSpawn.Count;
        waveTimer = waveDuration; 
    }

    public void GenerateEnemies()
    {

        List<GameObject> generatedEnemies = new List<GameObject>();
        while (WaveValue > 0 || generatedEnemies.Count < _maxEnemyAmount)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].Cost;

            if (WaveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].EnemyPrefab);
                WaveValue -= randEnemyCost;
            }
            else if (WaveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable] //otherwise i cant access them in the inspector
public class Enemy
{
    public GameObject EnemyPrefab;
    [Range(1, 30)]
    [Tooltip("This is the value of the enemy in the store.")]
    public int Cost;
}