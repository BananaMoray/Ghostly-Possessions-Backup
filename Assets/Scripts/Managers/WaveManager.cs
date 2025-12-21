using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    [SerializeField]
    private int _currentWave;
    public int WaveValue;
    public int WaveTotalValue;
    [SerializeField]
    private float _waveValueMultiplier = 1.5f;
    [SerializeField]
    private int _waveValueBase = 10;
    [SerializeField]
    private int _maxEnemyAmount = 25;

    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

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
                GameObject enemy = SpawnManager.Instance.SpawnPrefab(enemiesToSpawn[0], 40, 3, 1);

                //GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity);
                enemiesToSpawn.RemoveAt(0);
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;

                if (spawnIndex + 1 <= spawnLocation.Length - 1)
                {
                    spawnIndex++;
                }
                else
                {
                    spawnIndex = 0;
                }
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
            Debug.Log("New Wave");
            GenerateWave();
        }
    }

    public void GenerateWave()
    {
        WaveTotalValue = (int)(_waveValueBase + (Mathf.Pow(_waveValueMultiplier, _currentWave - 1)));
        WaveValue = WaveTotalValue;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }

    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.

        // repeat... 

        //  -> if we have no points left, leave the loop

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

[System.Serializable]
public class Enemy
{
    public GameObject EnemyPrefab;
    public int Cost;
}