using UnityEngine;
using UnityEngine.UI;

public class EquipmentPlatform : MonoBehaviour
{
    public enum UpgradeType
    {
        Jewelry,
        Tattoo
    }

    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private Image progressBar;
    [SerializeField] private float second;
    private float _time;
    private bool isProgressBarFilling;
    private int serialNo;
    private UpgradeStation upgradeStation;
    private AdUpgradeStation adUpgradeStation;
    private const string PlayerTag = "Player";

    private void Start()
    {
        upgradeStation = transform.parent.GetComponent<UpgradeStation>();

        if (upgradeStation != null)
        {
            serialNo = upgradeStation.GetSerialNo();
        }
        else
        {
            adUpgradeStation = transform.parent.GetComponent<AdUpgradeStation>();
            serialNo = adUpgradeStation.GetSerialNo();
        }
    }

    private void OnDisable()
    {
        progressBar.fillAmount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKey.FIRST_TIME_EQUIP + upgradeType + serialNo, 1) == 0)
        {
            isProgressBarFilling = true;
            _time = 0;  
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        if (isProgressBarFilling)
        {
            if (_time < second)
            {
                _time += Time.deltaTime;
                progressBar.fillAmount = _time / second;
            }
            else
            {
                isProgressBarFilling = false;
                EquipItem();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKey.FIRST_TIME_EQUIP + upgradeType + serialNo, 1) == 1)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_EQUIP + upgradeType + serialNo, 0);
        }
        
        isProgressBarFilling = false;
        progressBar.fillAmount = 0;
    }

    private void EquipItem()
    {
        if (upgradeType == UpgradeType.Jewelry)
        {
            UpgradesManager.Instance.ClearJewelryStation(PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_INDEX, 0));

            PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_AMOUNT, 1);
            PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_INDEX, serialNo);    
        }
        
        else if (upgradeType == UpgradeType.Tattoo)
        {
            UpgradesManager.Instance.ClearTattooStation(PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, 0));
                
            PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 1);
            PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, serialNo);
        }
        
        if (upgradeStation!=null)
        {
            upgradeStation.equipmentPlatform.gameObject.SetActive(false);
            upgradeStation.unequipmentPlatform.gameObject.SetActive(true);
        }
        else
        {
            adUpgradeStation.equipmentPlatform.gameObject.SetActive(false);
            adUpgradeStation.unequipmentPlatform.gameObject.SetActive(true);
        }
        
        PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + upgradeType + serialNo, 1);
    }
}