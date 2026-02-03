using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ship : MonoBehaviour, IPossessable
{
    [SerializeField]
    private ShipData _data;
    public ShipData Data => _data;

    public static event EventHandler<PossessEventArgs> OnSpaceShipDeath;

    private PlayerController owner;

    //[Header("Name of Spaceship")]
    //public string HUDSpaceShipName;
    //[Header("Name of Spaceship Weapon")]
    //public string HUDSpaceShipDescription;
    //[Header("ID of Spaceship Weapon")]
    //public int HUDWeaponID;

    //[Header("Movement Settings")]
    //public bool UseRelativeMovement = false;
    //public bool UseRelativeRotation = false;
    //[Header("Accelleration and Deceleration")]
    //[Tooltip("Increases Speed at which Velocity is being gained.")]
    //[SerializeField]
    //private float _acceleration = 15f;
    //[Tooltip("Increases Speed at which Velocity is being lost.")]
    //[SerializeField]
    //private float _deceleration = 8f;
    //[Header("Speed Variables")]
    //[SerializeField]
    //private float _maxSpeed = 6f;
    [Tooltip("Maximum Speed at which the player moves when boosting.")]
    [SerializeField]
    private float _boostSpeed = 10f;
    [Tooltip("Multiplier by which the HP is drained while boosting.")]
    [SerializeField]
    private float _AbilityHPDrainMultiplier = 3f;
    //[SerializeField]
    //private float _rotationSpeed = 360f;


    public Vector3 CurrentVelocity {  get; set; }



    //-----------------
    // Components
    //-----------------
    private MeshRenderer _renderer;
    private Material _normalMat;
    [SerializeField]
    private Material _possessMat;

    private Rigidbody _rb;

    private IShootable _shootComponent;

    public IHealth HealthComponent;

    public Vector2 MoveDirection;

    [Header("Particle Systems")]
    public ParticleSystem[] _thrusterParticleSystems;
    public List<ParticleSystem.EmissionModule> _thrusterEmission;
    [SerializeField]
    private GameObject _sparksPrefab;
    private GameObject _sparks;

    //[SerializeField]
    //private int _crossHairID;

    private void Awake()
    {
        Debug.Log(_data.ShipName);

        _rb = GetComponent<Rigidbody>();

        _renderer = GetComponent<MeshRenderer>();
        _normalMat = _renderer.material;

        _shootComponent = GetComponent<IShootable>();
        //if shootcomponent exists, override all the variables
        if (_shootComponent != null)
        {
            _shootComponent.DamageOverride = Data.Damage;
            _shootComponent.KnockbackOverride = Data.KnockBack;
            _shootComponent.SpeedOverride = Data.AttackSpeed;
            _shootComponent.LifeTimeOverride = Data.AttackLifeTime;
            _shootComponent.AttackDelayOverride = Data.AttackDelay;
        }

        _mainCamera = Camera.main;
        CalculateCameraDirections();

        _thrusterParticleSystems = GetComponentsInChildren<ParticleSystem>(true);

        foreach (ParticleSystem ps in _thrusterParticleSystems)
        {
            var em = ps.emission;
            em.enabled = false;
        }

        if (_rb != null)
        {
            _rb.freezeRotation = true;
        }

        HealthComponent = GetComponent<IHealth>();
        if (HealthComponent is HPLogic hpLogic)
        {
            //set health component health
            HealthComponent.MaxHealth = Data.Health;
            (HealthComponent as SpaceshipHPLogic).ResetHealth();
            //then afterwards create the health bar
            (HealthComponent as SpaceshipHPLogic).CreateHPBar();
            //subscribe to die event
            hpLogic.OnDied += HandleDeath;
        }
            

        if (_sparksPrefab != null)
        {
            _sparks = Instantiate(_sparksPrefab, transform.position, Quaternion.identity) as GameObject;
            _sparks.transform.SetParent(transform);
        }

    }

    public void OnStartPossess(PlayerController controller)
    {
        owner = controller;
        _renderer.material = _possessMat;
        gameObject.layer = 3;

        _sparks.SetActive(false);

        HealthComponent.HealthDrainEnabled = true;
        HealthComponent.SetHPBarActive(true);
        HealthComponent.SetOriginalColour();

        SetActiveCrosshair(true);

        HudScreenManager.Instance.SetHUD(Data.ShipName, Data.HUDWeaponName, Data.HUDWeaponID);
        HudScreenManager.Instance.SetHUDActive(true);
    }

    public void OnStopPossess()
    {
        owner = null;
        _renderer.material = _normalMat;
        if (_rb != null) _rb.linearVelocity = Vector2.zero;
        gameObject.layer = 0;

        EnableThrusters(false);
        HealthComponent.HealthDrainEnabled = false;
        HealthComponent.SetHPBarActive(false);
        HealthComponent.SetOriginalColour();

        SetActiveCrosshair(false);
        HudScreenManager.Instance.SetHUDActive(false);
    }

    private void HandleDeath()
    {
        OnSpaceShipDeath?.Invoke(this, new PossessEventArgs(owner.gameObject));

        if (owner != null)
            owner.UnpossessObject();
    }

    public Transform GetPossessionTransform()
    {
        return transform;
    }

    public void HandlePossessedInput(Vector2 moveInput, Vector2 lookInput)
    {
        HandleMovement(moveInput);

        HandleRotation(moveInput, lookInput);



        _rb.linearVelocity = CurrentVelocity;

        EnableThrusters(_isBoosting);
    }

    private void LateUpdate()
    {

    }

    private void SetActiveCrosshair(bool istrue)
    {
        GameManager.CrosshairObjects[(int)Data.CrossHairVisual].SetActive(istrue);
    }

    private void HandleCrossHairTransform(int i)
    {
        GameManager.CrosshairObjects[i].transform.position = transform.position;
        GameManager.CrosshairObjects[i].transform.rotation = transform.rotation;
    }

    private void HandleMovement(Vector2 moveInput)
    {
        if (_rb == null) return;

        Vector3 inputVelocity = Vector3.zero;

        //Absolute-Movement
        inputVelocity = new Vector3(moveInput.x, 0, moveInput.y) * Data.MaxSpeed;

        if (_isBoosting)
        {
            float x = moveInput.x;
            float z = _isBoosting == true ? 1 : 0;

            inputVelocity = (transform.right * x) + (transform.forward * z * _boostSpeed);

        }

        CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, inputVelocity, Data.Acceleration * Time.deltaTime);

        if (moveInput.magnitude < 0.01f)
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, Data.Deceleration * Time.deltaTime);
        }
    }

    private void EnableThrusters(bool b)
    {
        if (HealthComponent != null)
            (HealthComponent as HPLogic).HpDrainRateInSeconds = b ? 1 / _AbilityHPDrainMultiplier : 1;

        if (_thrusterParticleSystems.Length != 0)
        {
            foreach (var ps in _thrusterParticleSystems)
            {
                var em = ps.emission;
                em.enabled = b;
            }
        }
    }

    private bool _isBoosting;

    public virtual void HandlePossessedBoost(bool boostInput)
    {
        _isBoosting = boostInput;
    }

    //camera stuff
    private Camera _mainCamera;
    private Vector3 _cameraUp;
    private Vector3 _cameraRight;

    private void CalculateCameraDirections()
    {
        if (_mainCamera == null) return;

        _cameraUp = _mainCamera.transform.up;
        _cameraRight = _mainCamera.transform.right;
        _cameraUp.y = 0;
        _cameraRight.y = 0;
        _cameraUp.Normalize();
        _cameraRight.Normalize();
    }

    Vector3 _lookDirection = Vector3.zero;

    public void HandleRotation(Vector2 moveInput, Vector2 lookInput)
    {

        if (!_isBoosting)
        {
            if (lookInput.sqrMagnitude > 0.01f)
                _lookDirection = (_cameraUp * lookInput.y + _cameraRight * lookInput.x);
        }
        else
        {
            if (moveInput.sqrMagnitude > 0.01f)
                _lookDirection = (_cameraUp * moveInput.y + _cameraRight * moveInput.x);
        }

        if (_lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Data.RotationSpeed * Time.deltaTime);
        }
    }


    public virtual void HandlePossessedAttack(bool attackInput)
    {
        if (_shootComponent != null)
        {
            _shootComponent.OnRequestAttack(attackInput);
        }
    }

    public void HandlePossessedLateUpdate()
    {
        HandleCrossHairTransform((int)Data.CrossHairVisual);
    }
}