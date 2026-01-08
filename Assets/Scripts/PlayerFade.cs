using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class PlayerFade : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;
    }

    public void FadeOut(float duration)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeTo(0f, duration));
    }

    public void FadeIn(float duration)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeTo(1f, duration));
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color startColor = _material.color;
        float startAlpha = startColor.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            _material.color = newColor;

            yield return null;
        }

        Color finalColor = _material.color;
        finalColor.a = targetAlpha;
        _material.color = finalColor;
    }
}
