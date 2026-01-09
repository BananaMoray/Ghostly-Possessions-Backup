using System;
using TMPro;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    [SerializeField]
    private string[] _hints;

    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        RandomiseTip();
    }

    public void RandomiseTip()
    {
        int roll = UnityEngine.Random.Range(0, _hints.Length);

        _textMeshPro.text = _hints[roll];
    }

    public void PlayLoadingScreen()
    {
        
    }
}
