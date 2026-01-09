using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudScreenManager : MonoBehaviour
{
    public static HudScreenManager Instance;

    [SerializeField]
    private GameObject _hudObject;
    [SerializeField]
    private TextMeshProUGUI _shipName;
    [SerializeField]
    private TextMeshProUGUI _weaponDesc;
    [SerializeField]
    private Image _weaponImage;

    public Sprite[] WeaponSprites;

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
        {
            Instance = this;
        }
        SetHUDActive(false);

    }

    public void SetHUD(string name, string weaponName, int weaponID)
    {
        _shipName.text = name;
        _weaponDesc.text = weaponName;
        _weaponImage.sprite = WeaponSprites[weaponID];
    }

    public void SetHUDActive(bool isTrue)
    {
        _hudObject.SetActive(isTrue);
    }
}
