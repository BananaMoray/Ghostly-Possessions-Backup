using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SceneManager _sceneManager;

    public static int WaveCount;

    public static bool DebugMode;

    public  bool DebugModeEnabled;

    public int MaxShipLimit = 10;

    //public static List<GameObject> PossessableShips = new List<GameObject>();
    //public static List<int> ShipQualites = new List<int>();

    public static int MaxPossessableShips = 10;

    //public static int LowestShipQuality = 0;

    public GameObject[] CrosshairPrefabs;

    public GameObject[] CanvasWeaponPrefabs;

    public static GameObject[] CrosshairObjects;

    public static Dictionary<GameObject, int> PossessableShipsDictionary = new Dictionary<GameObject , int>();

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CrosshairObjects = new GameObject[CrosshairPrefabs.Length];

        for (int i = 0; i < CrosshairPrefabs.Length; i++)
        {
            CrosshairObjects[i] = Instantiate(CrosshairPrefabs[i]);
            CrosshairObjects[i].SetActive(false);
        }

        MaxPossessableShips = MaxShipLimit;

        DebugMode = DebugModeEnabled;
        //ShipQualites.Add(0);
    }


    public static int ReturnHighestQuality()
    {
        int maxValue = int.MinValue;

        foreach (KeyValuePair<GameObject, int> ship in PossessableShipsDictionary)
        {
            maxValue = Math.Max(maxValue, ship.Value);
        }

        return maxValue;
    }

    public static int ReturnLowestQuality()
    {
        int minValue = int.MaxValue;

        foreach (KeyValuePair<GameObject, int> ship in PossessableShipsDictionary)
        {
            minValue = Math.Min(minValue, ship.Value);

        }

        return minValue;
    }



}
