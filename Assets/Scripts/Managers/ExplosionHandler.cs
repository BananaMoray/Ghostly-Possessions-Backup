using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    private float _lifeTime = 2f;
    private float _currentLife;

    private void Awake()
    {
        ScreenShakeManager.ShakeScreen(.4f, .5f);
        HitStopManager.HitStop(0.1f);
    }
    private void Update()
    {
        _currentLife += Time.deltaTime;

        if (_currentLife >= _lifeTime )
            Destroy(gameObject);
    }
}
