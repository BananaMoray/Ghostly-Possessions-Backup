using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{

    void Start()
    {
        RandomiseRotation();
    }

    private void RandomiseRotation()
    {
        transform.rotation = Random.rotation;
    }
}
