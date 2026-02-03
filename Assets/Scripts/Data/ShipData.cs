using UnityEngine;

public enum ShipType
{
    None,
    Automatic,
    Laser,
    Shotgun,
    Special
}

[CreateAssetMenu(fileName = "ShipData", menuName = "GalvasiteData/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("General Data")]
    [SerializeField]
    [Tooltip("Name of Spaceship on HUD.")]
    private string _ShipName = "...";
    public string ShipName => _ShipName;

    [SerializeField]
    [Range(0, 6)]
    private int _shipQuality = 1;
    public int ShipQuality => _shipQuality;

    [Header("HUD Data")]

    [SerializeField]
    [Tooltip("Name of Spaceship Weapon on HUD.")]
    private string _hudWeaponName = "...";
    public string HUDWeaponName => _hudWeaponName;

    [SerializeField]
    private int _hudWeaponID = 0;
    public int HUDWeaponID => _hudWeaponID;

    [SerializeField]
    private int _crossHairID = 0;
    public int CrossHairID => _crossHairID;

    //-------------------------------------
    [Header("Health Data")]
    [SerializeField]
    private float _health = 100f;
    public float Health => _health;

    [Header("Movement Data")]
    [SerializeField]
    private float _maxSpeed = 5.0f;
    public float MaxSpeed => _maxSpeed;

    [SerializeField]
    private float _acceleration = 10.0f;
    public float Acceleration => _acceleration;

    [SerializeField]
    private float _deceleration = 5.0f;
    public float Deceleration => _deceleration;

    [SerializeField]
    private float _rotationSpeed = 720.0f;
    public float RotationSpeed => _rotationSpeed;

    [Header("Attack Data")]
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



}
