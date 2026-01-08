using System.Collections;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    private static ScreenShakeManager Instance;

    [SerializeField]
    private AnimationCurve _animationCurve;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public static void ShakeScreen(float strength, float duration)
    {
        Instance.StartCoroutine(Instance.ShakeRoutine(strength, duration));
    }

    private IEnumerator ShakeRoutine(float strength, float duration)
    {
        //Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float shakeStrength = _animationCurve.Evaluate(elapsedTime /duration) * strength;
            transform.localPosition += Random.insideUnitSphere * shakeStrength;
            yield return null;
        }
        //transform.localPosition = Vector3.zero;
    }
}
