using System;
using UnityEngine;
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
    //[SerializeField]
    //private int _spawnCount = 20;
    [SerializeField]
    private LayerMask _losMask;

    private void Awake()
    {
        //SpawnAsteroids();
        if (Instance == null)
            Instance = this;
    }

    public GameObject SpawnPrefab(GameObject prefab, float radius, float checkRadius, int spawnCount, bool respectPlayerRadius)
    {
        GameObject gObject = null;
        //bool isValidPosition = false;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = RandomisePosition(radius);

            Collider[] hits = Physics.OverlapSphere(spawnPos, checkRadius, _losMask);

            while (hits.Length > 0)
                RandomisePosition(radius);

            gObject = Instantiate(prefab, spawnPos, Quaternion.identity);
        }


        return gObject;
    }

    private Vector3 RandomisePosition(float radius)
    {
        Vector3 pos = Random.insideUnitSphere * radius;

        pos.y = 0;

        return pos;
    }
}
