
using System;
using UnityEngine;
using UnityEngine.Audio;

public class BaseShootClass : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject _damagerPrefab;

    [SerializeField]
    private AudioResource _shootsfx;

    [SerializeField]
    private GameObject[] _barrels;

    [Header("Override Values")]

    [SerializeField]
    protected float _dmgOverride;
    public float DamageOverride
    {
        get { return _dmgOverride; }
        set { _dmgOverride = value; }
    }

    [SerializeField]
    protected float _knockbackOverride;
    public float KnockbackOverride
    {
        get { return _knockbackOverride; }
        set { _knockbackOverride = value; }
    }

    [SerializeField]
    protected float _speedOverride;
    public float SpeedOverride
    {
        get { return _speedOverride; }
        set { _speedOverride = value; }
    }

    [SerializeField]
    protected float _lifeTimeOverride;
    public float LifeTimeOverride
    {
        get { return _lifeTimeOverride; }
        set { _lifeTimeOverride = value; }
    }

    [Header("Screenshake Variables")]
    [SerializeField]
    [Range(0, 3f)]
    private float _screenShakeAmount = 0f;

    [SerializeField]
    [Range(0, 1f)]
    private float _screenShakeDuration = 0f;

    private void Awake()
    {
        //_barrels = GameObject.Tag("Barrel");

        //_barrels.Add(GameObject.FindGameObjectsWithTag("Barrel)"));

        //_shootsfx = GetComponent<AudioSource>();
        //Debug.Log(_shootsfx.name);
    }

    public virtual void Attack()
    {
        SpawnDamager(transform.rotation);
    }

    public virtual void Attack(int amount, float angle)
    {
        float halfAngle = angle / 2;

        for (int i = 0; i < amount; i++)
        {

            float t;

            //first bullet is always the most left bullet
            if (amount == 1)
            {
                t = 0;
            }
            else
            {
                t = (float)i / (amount - 1);
            }

            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);

            Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, currentAngle, 0));

            SpawnDamager(rotation);
        }
    }

    private Vector3 _shotOrigin;

    public GameObject SpawnDamager(Quaternion rot)
    {
        DetermineCurrentBarrel();

        if (_screenShakeAmount >  0f)
            ScreenShakeManager.ShakeScreen(_screenShakeAmount, _screenShakeDuration);

        GameObject damagerObj = Instantiate(_damagerPrefab, _shotOrigin, rot);

        if (_shootsfx != null)
            SoundManager.Instance.PlaySoundFXClip(_shootsfx, transform);

        IDamager damager = damagerObj.GetComponent<IDamager>();

        if (damager == null)
        {
            Debug.LogError("no damager attached to script!");
            return null;
        }

        damager.Damage = DamageOverride;
        damager.KnockbackStrength = KnockbackOverride;
        damager.LifeTime = LifeTimeOverride;
        damager.Speed = SpeedOverride;

        return damagerObj;
    }

    private int _currentBarrel;

    private void DetermineCurrentBarrel()
    {
        if (_barrels.Length >= 1)
        {
            _shotOrigin = _barrels[_currentBarrel].transform.position;

            _currentBarrel++;

            if (_currentBarrel >= _barrels.Length)
                _currentBarrel = 0;
        }
        else
            _shotOrigin = transform.position;
    }

    public virtual void OnRequestAttack(bool attack)
    {

    }
}
