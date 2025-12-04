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
        UpdateBarColor();
    }

    protected override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        if (_hpSlider != null)
            _hpSlider.value = GetCurrentHealthPercent();

        UpdateBarColor();
    }

    public override void SetHPBarActive(bool b)
    {
        if (_hpBar != null)
        {
            _hpBar.SetActive(b);
        }
    }

    private void UpdateBarColor()
    {
        if (_fillImage == null) return;

        float hpPercent = GetCurrentHealthPercent();

        _fillImage.color = Color.Lerp(_lowColour, _fullColour, hpPercent);
    }

    protected override void Die()
    {
        if (_hpBar != null)
            Destroy(_hpBar);

        base.Die();
    }
}
