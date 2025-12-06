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
    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (spawnTimer <= 0)
        //{
        //    //spawn an enemy
        //    if (enemiesToSpawn.Count > 0)
        //    {
        //        GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
        //        enemiesToSpawn.RemoveAt(0); // and remove it
        //        spawnedEnemies.Add(enemy);
        //        spawnTimer = spawnInterval;

        //        if (spawnIndex + 1 <= spawnLocation.Length - 1)
        //        {
        //            spawnIndex++;
        //        }
        //        else
        //        {
        //            spawnIndex = 0;
        //        }
        //    }
        //    else
        //    {
        //        waveTimer = 0; // if no enemies remain, end wave
        //    }
        //}
        //else
        //{
        //    spawnTimer -= Time.fixedDeltaTime;
        //    waveTimer -= Time.fixedDeltaTime;
        //}

        //if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
        //{
        //    _currentWave++;
        //    GenerateWave();
        //}
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