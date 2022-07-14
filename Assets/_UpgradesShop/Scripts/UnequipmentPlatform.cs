using UnityEngine;
using UnityEngine.UI;

public class UnequipmentPlatform : MonoBehaviour
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

        if (upgradeStation!= null)
        {
            serialNo = upgradeStation.GetSerialNo();
        }
        else
        {
            adUpgradeStation = transform.parent.GetComponent<AdUpgradeStation>();
            serialNo = adUpgradeStation.GetSerialNo();
        }
    }

    public int GetSerialNo()
    {
        return serialNo;
    }

    public UpgradeType GetUpgradeType()
    {
        return upgradeType;
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

        if (PlayerPrefs.GetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + upgradeType + serialNo, 1) == 0)
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
                UnequipItem();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }
        
        if (PlayerPrefs.GetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + upgradeType + serialNo, 1) == 1)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + upgradeType + serialNo, 0);
        }
        
        isProgressBarFilling = false;
        progressBar.fillAmount = 0;
    }

    private void UnequipItem()
    {
        if (upgradeType == UpgradeType.Jewelry)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_AMOUNT, 0);    
        }
        else if (upgradeType == UpgradeType.Tattoo)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 0);
        }
        
        if (upgradeStation!=null)
        {
            upgradeStation.equipmentPlatform.gameObject.SetActive(true);
            upgradeStation.unequipmentPlatform.gameObject.SetActive(false);
        }
        else
        {
            adUpgradeStation.equipmentPlatform.gameObject.SetActive(true);
            adUpgradeStation.unequipmentPlatform.gameObject.SetActive(false);
        }

        PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_EQUIP + upgradeType + serialNo, 1);
    }
}
