using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    private float _lifeTime = 2f;
    private float _currentLife;
    private void Update()
    {
        _currentLife += Time.deltaTime;

        if (_currentLife >= _lifeTime )
            Destroy(gameObject);
    }
}
