using System.Collections;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    private static ScreenShakeManager _screenShakeInstance;

    [SerializeField]
    private AnimationCurve _animationCurve;


    private void Awake()
    {
        if (_screenShakeInstance == null)
            _screenShakeInstance = this;
    }

    public static void ShakeScreen(float strength, float duration)
    {
        _screenShakeInstance.StartCoroutine(_screenShakeInstance.ShakeRoutine(strength, duration));
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
