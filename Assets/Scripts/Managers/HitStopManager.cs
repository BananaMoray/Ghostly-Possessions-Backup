using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    private static HitStopManager Instance;
    private bool _isHitStopping = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    public static void HitStop(float duration)
    {
        //singelton lmaoooooo
        if (Instance != null)
            Instance.StartCoroutine(Instance.DoHitStop(duration));
    }

    private IEnumerator DoHitStop(float duration)
    {
        if (_isHitStopping)
            yield break;

        _isHitStopping = true;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;

        _isHitStopping = false;
    }
}
