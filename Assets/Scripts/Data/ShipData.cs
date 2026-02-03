using UnityEditor;
using UnityEngine;

public enum ShipType
{
    None,
    Automatic,
    Laser,
    Shotgun,
    Special
}

public enum CrossHairType
{
    Normal = 0,
    Target = 1,
    Shotgun = 2,
    Laser = 3,
    Wave = 4
}

[CreateAssetMenu(fileName = "ShipData", menuName = "GalvasiteData/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("General Data")]
    [SerializeField]
    [Tooltip("Name of Spaceship on HUD.")]
    private string _ShipName = "...";
    public string ShipName
    {
        get { return _ShipName; }
        set { _ShipName = value; }
    }

    [SerializeField]
    [Range(0, 6)]
    private int _shipQuality = 1;
    public int ShipQuality
    {
        get { return _shipQuality; }
        set { _shipQuality = value; }
    }

    [Header("HUD Data")]

    [SerializeField]
    [Tooltip("Name of Spaceship Weapon on HUD.")]
    private string _hudWeaponName = "...";
    public string HUDWeaponName
    {
        get { return _hudWeaponName; }
        set { _hudWeaponName = value; }
    }

    [SerializeField]
    private int _hudWeaponID = 0;
    public int HUDWeaponID
    {
        get { return _hudWeaponID; }
        set { _hudWeaponID = value; }
    }

    [SerializeField]
    private CrossHairType _crossHairVisual = CrossHairType.Normal;
    public CrossHairType CrossHairVisual
    {
        get { return _crossHairVisual; }
        set { _crossHairVisual = value; }
    }

    //-------------------------------------
    [Header("Health Data")]
    [SerializeField]
    private float _health = 100f;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    [Header("Movement Data")]
    [SerializeField]
    private float _maxSpeed = 5.0f;
    public float MaxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; }
    }

    [SerializeField]
    private float _acceleration = 10.0f;
    public float Acceleration
    {
        get { return _acceleration; }
        set { _acceleration = value; }
    }

    [SerializeField]
    private float _deceleration = 5.0f;
    public float Deceleration
    {
        get { return _deceleration; }
        set { _deceleration = value; }
    }

    [SerializeField]
    private float _rotationSpeed = 720.0f;
    public float RotationSpeed
    {
        get { return _rotationSpeed; }
        set { _rotationSpeed = value; }
    }

    [Header("Attack Data")]
    [SerializeField]
    private float _damage = 5.0f;
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField]
    private float _knockBack = 5.0f;
    public float KnockBack
    {
        get { return _knockBack; }
        set { _knockBack = value; }
    }

    [SerializeField]
    private float _attackSpeed = 40.0f;
    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    [SerializeField]
    private float _attackLifeTime = 2.0f;
    public float AttackLifeTime
    {
        get { return _attackLifeTime; }
        set { _attackLifeTime = value; }
    }

    [SerializeField]
    private float _attackDelay = 0.5f;
    public float AttackDelay
    {
        get { return _attackDelay; }
        set { _attackDelay = value; }
    }

    [Header("Screenshake")]
    [SerializeField]
    [Range(0, 3f)]
    private float _screenShakeIntensity = 0.0f;
    public float ScreenShakeIntensity
    {
        get { return _screenShakeIntensity; }
        set { _screenShakeIntensity = value; }
    }

    [SerializeField]
    [Range(0, 1f)]
    private float _screenShakeDuration = 0.0f;
    public float ScreenShakeDuration
    {
        get { return _screenShakeDuration; }
        set { _screenShakeDuration = value; }
    }
}
