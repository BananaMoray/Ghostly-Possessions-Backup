using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHPLogic : HPLogic
{
    [Header("HP Bar")]
    [SerializeField]
    private GameObject _hpBarPrefab;
    private GameObject _hpBar;
    private Slider _hpSlider;
    private Image _fillImage;

    private Color _fullColour = Color.white;
    //private Color _midColor = Color.yellow;
    private Color _lowColour = Color.red;

    protected override void Awake()
    {
        base.Awake();

        if (_hpBarPrefab != null)
            CreateHPBar();
    }

    private void CreateHPBar()
    {
        //instantiate HPBAr
        _hpBar = Instantiate(_hpBarPrefab, transform.position, Quaternion.identity);

        HPBarController controller = _hpBar.GetComponent<HPBarController>();
        controller.owner = gameObject;

        _hpSlider = _hpBar.GetComponentInChildren<Slider>();
        _fillImage = _hpSlider.fillRect.GetComponent<Image>();

        //let's calculate some odd HPBar scaling
        RectTransform rect = _hpSlider.GetComponent<RectTransform>();
        float hpWidth = _maxHealth / 2f;
        float scaleFactor = Mathf.Clamp(_maxHealth / 2f, 1f, 120f);
        rect.sizeDelta = new Vector2(hpWidth, rect.sizeDelta.y);

        _hpSlider.value = 1f;
        SetHPBarActive(false);
        UpdateHUD();
    }

    protected override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);


        UpdateHUD();
    }

    public override void SetHPBarActive(bool b)
    {
        if (_hpBar != null)
        {
            _hpBar.SetActive(b);
        }
    }

    private void UpdateHUD()
    {
        if (_fillImage == null) return;

        float hpPercent = GetCurrentHealthPercent();

        if (_hpSlider != null)
            _hpSlider.value = GetCurrentHealthPercent();

        _fillImage.color = Color.Lerp(_lowColour, _fullColour, hpPercent);
    }

    private bool _isDying = false;

    protected override void ExplodeOnDeath()
    {
        if (_hpBar != null)
            Destroy(_hpBar);

        if (!_isDying)
        {
            GameManager.PossessableShipsDictionary.Remove(gameObject);
            //GameManager.PossessableShips.Remove(this.gameObject);
            StartCoroutine(ExplodeDelayRoutine());
        }
            
    }

    public IEnumerator ExplodeDelayRoutine()
    {
        _isDying = true;

        _meshRenderer.material = _damageMat;
        SoundManager.Instance.PlaySoundFXClip(_damageSFX, transform, SoundManager.SFXVolume);

        yield return new WaitForSeconds(0.4f);

        _meshRenderer.material = _originMat;

        yield return new WaitForSeconds(0.4f);

        _meshRenderer.material = _damageMat;
        SoundManager.Instance.PlaySoundFXClip(_damageSFX, transform, SoundManager.SFXVolume);

        yield return new WaitForSeconds(0.4f);

        _meshRenderer.material = _originMat;

        yield return new WaitForSeconds(0.4f);

        _meshRenderer.material = _damageMat;
        SoundManager.Instance.PlaySoundFXClip(_damageSFX, transform, SoundManager.SFXVolume);

        yield return new WaitForSeconds(0.4f);

        _meshRenderer.material = _originMat;

        yield return new WaitForSeconds(0.4f);

        HitStopManager.HitStop(0.1f);

        base.ExplodeOnDeath();
    }
}
