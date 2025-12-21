using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class ChargeShoot : BaseShootClass
{
    [SerializeField]
    private float _chargeTime = 1f;
    private float _chargeTimer;
    private bool _canAttack = false;


    [SerializeField]
    private GameObject _chargePrefab;
    private Slider _chargeSlider;


    private void Awake()
    {
        _chargePrefab.SetActive(false);
        _chargeSlider = _chargePrefab.GetComponentInChildren<Slider>();
    }

    public override void OnRequestAttack(bool attack)
    {
        if (attack)
        {
            _chargePrefab.SetActive(true);
            _chargeTimer += Time.deltaTime;

            if (_chargeTimer >= _chargeTime)
            {
                _chargeTimer = _chargeTime;
                _canAttack = true;
            }
        }

        if (!attack)
        {
            _chargePrefab.SetActive(false);

            if (_canAttack)
            {
                Attack();
            }
            else
            {
                
                _chargeTimer -= Time.deltaTime;
            }

            _chargePrefab.SetActive(false);

        }

        if (_chargeTimer <= 0f)
        {
            _chargeTimer = 0f;
        }
    }

    private void Update()
    {
        HandleUI();
    }

    private void HandleUI()
    {
        _chargeSlider.value = _chargeTimer / _chargeTime;
        _chargePrefab.transform.position = transform.position;
        _chargePrefab.transform.rotation = Quaternion.identity;
    }

    public override void Attack()
    {
        base.Attack();
        _canAttack = false;
        _chargeTimer = 0;
    }

    public override void Attack(int amount, float angle)
    {
        base.Attack(amount, angle);
    }
}
