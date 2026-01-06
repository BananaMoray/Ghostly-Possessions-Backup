using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SceneManager _sceneManager;

    public GameObject[] waves;

    public int MaxShipLimit = 10;

    public static List<GameObject> PossessableSpaceShips = new List<GameObject>();

    public static int MaxPossessableShips = 10;

    public static int LowestShipQuality = 0;

    public GameObject[] CrosshairPrefabs;

    public static GameObject[] CrosshairObjects;

    private void Start()
    {
        CrosshairObjects = new GameObject[CrosshairPrefabs.Length];

        for (int i = 0; i < CrosshairPrefabs.Length; i++)
        {
            CrosshairObjects[i] = Instantiate(CrosshairPrefabs[i]);
            CrosshairObjects[i].SetActive(false);
        }

        MaxPossessableShips = MaxShipLimit;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            LowestShipQuality = 0;
            PossessableSpaceShips = new List<GameObject>();
        }
    }

}
