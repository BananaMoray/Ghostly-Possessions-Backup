using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    [SerializeField]
    private string[] _hints;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayLoadingScreen()
    {

    }
}
