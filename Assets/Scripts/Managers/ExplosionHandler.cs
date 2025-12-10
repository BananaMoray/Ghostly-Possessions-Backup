using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    private float _lifeTime = 2f;
    private float _currentLife;

    [SerializeField]
    [Range(0, 2f)]
    private float _hitStopTime = .5f;
    [SerializeField]
    [Range(0, 2f)]
    private float _shakeAmount = 1.5f;
    [SerializeField]
    [Range(0, 1f)]
    private float _shakeTime = .3f;

    private void Awake()
    {
        HitStopManager.HitStop(_hitStopTime);
        ScreenShakeManager.ShakeScreen(_shakeAmount, _shakeTime);
    }
    private void Update()
    {
        _currentLife += Time.deltaTime;

        if (_currentLife >= _lifeTime )
            Destroy(gameObject);
    }
}
