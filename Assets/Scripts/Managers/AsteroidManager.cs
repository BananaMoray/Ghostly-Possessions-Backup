using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private float _spawnRadius = 40f;
    [SerializeField]
    private float _checkRadius = 2f;
    [SerializeField]
    private int _spawnCount = 30;
    [SerializeField]
    private bool _respectPlayer = false;


    public List<GameObject> Asteroids = new List<GameObject>();

    void Start()
    {
        for (int i = 0;  i < _spawnCount; i++)
        {
            GameObject asteroid = (GameObject)SpawnManager.Instance.InstantiatePrefab(_asteroidPrefab, _spawnRadius, _checkRadius, _respectPlayer);
            Asteroids.Add(asteroid);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
