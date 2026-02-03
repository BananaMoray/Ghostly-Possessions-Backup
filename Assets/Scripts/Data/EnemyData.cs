using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShipData", menuName = "GalvasiteData/EnemyShipData")]
public class EnemyShipData : ScriptableObject
{
    [Header("General Data")]
    [SerializeField]
    [Tooltip("Name of Spaceship Internally.")]
    private string _enemyShipName = "...";
    public string ShipName => _enemyShipName;

    [SerializeField]
    [Tooltip("Quality of Spaceship Drop.")]
    [Range(0, 6)]
    private int _shipQuality = 1;
    public int ShipQuality => _shipQuality;

    //-------------------------------------
    [Header("Enemy Health Data")]
    [SerializeField]
    private float _enemyHealth = 100f;
    public float EnemyHealth => _enemyHealth;

    [Header("Enemy Movement Data")]
    [SerializeField]
    private float _enemyMaxSpeed = 5.0f;
    public float EnemyMaxSpeed => _enemyMaxSpeed;

    [SerializeField]
    private float _enemyAcceleration = 10.0f;
    public float EnemyAcceleration => _enemyAcceleration;

    [SerializeField]
    private float _enemyDeceleration = 5.0f;
    public float EnemyDeceleration => _enemyDeceleration;

    [SerializeField]
    private float _enemyRotationSpeed = 360.0f;
    public float EnemyRotationSpeed => _enemyRotationSpeed;

    [Header("Enemy Attack Data")]
    [SerializeField]
    private float _attackRange = 12.0f;
    public float AttackRange => _attackRange;
    [SerializeField]
    private float _damage = 5.0f;
    public float Damage => _damage;

    [SerializeField]
    private float _knockBack = 5.0f;
    public float KnockBack => _knockBack;

    [SerializeField]
    private float _attackSpeed = 40.0f;
    public float AttackSpeed => _attackSpeed;

    [SerializeField]
    private float _attackLifeTime = 2.0f;
    public float AttackLifeTime => _attackLifeTime;

    [SerializeField]
    private float _attackDelay = 0.5f;
    public float AttackDelay => _attackDelay;

    [Header("Screenshake")]
    [SerializeField]
    [Range(0, 3f)]
    private float _screenShakeIntensity = 0.0f;
    public float ScreenShakeIntensity => _screenShakeIntensity;

    [SerializeField]
    [Range(0, 1f)]
    private float _screenShakeDuration = 0.0f;
    public float ScreenShakeDuration => _screenShakeDuration;
}
