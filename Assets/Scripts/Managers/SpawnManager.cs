using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    //[SerializeField]
    //private GameObject _asteroidPrefab;
    //[SerializeField]
    //private float _spawnRadius = 20f;
    //[SerializeField]
    //private float _checkRadius = 2f;
    [SerializeField]
    private float _playerDistanceCheck = 10f;
    [SerializeField]
    private LayerMask _losMask;
    private GameObject _player;

    private void Awake()
    {
        //SpawnAsteroids();
        if (Instance == null)
            Instance = this;


        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject InstantiatePrefabs(GameObject prefab, float radius, float checkRadius, int spawnCount, bool respectPlayerRadius)
    {
        GameObject lastSpawned = null;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = Vector3.zero;
            bool validPosition = false;

            int attempts = 0;
            int maxAttempts = 50;

            while (!validPosition && attempts < maxAttempts)
            {
                spawnPos = RandomisePosition(radius);

                Collider[] hits = Physics.OverlapSphere(spawnPos, checkRadius, _losMask);


                validPosition = hits.Length == 0;

                if (respectPlayerRadius)
                {
                    float distToPlayer = Vector3.Distance(spawnPos, _player.transform.position);

                    if (distToPlayer < _playerDistanceCheck)
                    {
                        validPosition = false;
                        Debug.Log("Too close to player");
                    }

                }

                attempts++;
            }

            if (!validPosition)
            {
                Debug.LogWarning("scary scary no spawn position possible");
                continue;
            }



            lastSpawned = Instantiate(prefab, spawnPos, Quaternion.identity);
        }

        return lastSpawned;
    }

    public GameObject InstantiatePrefab(GameObject prefab, float radius, float checkRadius, bool respectPlayerRadius)
    {
        GameObject lastSpawned = null;


        Vector3 spawnPos = Vector3.zero;
        bool validPosition = false;

        int attempts = 0;
        int maxAttempts = 50;

        while (!validPosition && attempts < maxAttempts)
        {
            spawnPos = RandomisePosition(radius);

            Collider[] hits = Physics.OverlapSphere(spawnPos, checkRadius, _losMask);


            validPosition = hits.Length == 0;

            if (respectPlayerRadius)
            {
                float distToPlayer = Vector3.Distance(spawnPos, _player.transform.position);

                if (distToPlayer < _playerDistanceCheck)
                {
                    validPosition = false;
                    //Debug.Log("Too close to player");
                }

            }

            attempts++;
        }

        lastSpawned = Instantiate(prefab, spawnPos, Quaternion.identity);


        return lastSpawned;
    }

    private Vector3 RandomisePosition(float radius)
    {
        float xpos = Random.Range(-radius, radius);
        float ypos = Random.Range(-radius, radius);

        Vector3 playerPos = _player.transform.position;
        Vector3 spawnPos = new Vector3(xpos, 0, ypos);

        //Debug.Log(playerPos + " " + spawnPos);

        return playerPos + spawnPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
    }
}
