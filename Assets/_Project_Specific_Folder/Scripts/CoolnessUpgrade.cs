using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoolnessUpgrade : MonoBehaviour
{
    [SerializeField] private Sprite normalCoolnessUpgradeIcon;
    [SerializeField] private Sprite watchAdCoolnessUpgradeIcon;
    [SerializeField] private int requiredScoreForCoolnessUpgrade;
    [SerializeField] private int startingLevelForUpgradeCoolnessWatchingAd;

    private Image _coolnessUpgradeButtonImage;
    private Button _button;
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _levelText;
    private bool _isAdEnabled;

    private void Start()
    {
        _button = transform.GetComponent<Button>();
        _coolnessUpgradeButtonImage = transform.GetComponent<Image>();
        _costText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _levelText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _levelText.SetText("Stage " + PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1));

        CheckCoolnessUpgradeButtonTypeStatus();

        if (!_isAdEnabled)
        {
            CheckCoolnessUpgradeButtonAvailability();
        }

        if (PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1) == GameManager.Instance.GetTotalTattooGunAmount())
        {
            DisableCoolnessUpgradeButton();
        }
    }

    private void CheckCoolnessUpgradeButtonTypeStatus()
    {
        if (PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1) >= startingLevelForUpgradeCoolnessWatchingAd)
        {
            _isAdEnabled = true;
            _coolnessUpgradeButtonImage.sprite = watchAdCoolnessUpgradeIcon;
            _costText.gameObject.SetActive(false);
        }
        else
        {
            _isAdEnabled = false;
            _coolnessUpgradeButtonImage.sprite = normalCoolnessUpgradeIcon;
            _costText.gameObject.SetActive(true);
            _costText.SetText("$" + requiredScoreForCoolnessUpgrade);
        }
    }

    private void CheckCoolnessUpgradeButtonAvailability()
    {
        if (StorageManager.GetTotalScore() >= requiredScoreForCoolnessUpgrade)
        {
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }

    private void DisableCoolnessUpgradeButton()
    {
        _button.interactable = false;
    }

    public void OnCoolnessUpgradeButtonClick()
    {
        int currentCoolnessLevel = PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1);
        
        if (currentCoolnessLevel < GameManager.Instance.GetTotalTattooGunAmount())
        {
            currentCoolnessLevel += 1;

            if (!_isAdEnabled)
            {
                if (StorageManager.GetTotalScore() >= requiredScoreForCoolnessUpgrade)
                {
                    StorageManager.SetTotalScore(StorageManager.GetTotalScore() - requiredScoreForCoolnessUpgrade);
                    
                    UiManager.Instance.UpdateTotalScoreText(StorageManager.GetTotalScore());
                    
                    CoolnessUpgradeButtonEffects(currentCoolnessLevel);
                }
                CheckCoolnessUpgradeButtonAvailability();    
            }
            else
            {
                Debug.Log("Ad Watched");
                
                CoolnessUpgradeButtonEffects(currentCoolnessLevel);
            }
            
            CheckCoolnessUpgradeButtonTypeStatus();
        }
    }

    private void CoolnessUpgradeButtonEffects(int coolnessLevel)
    {
        GameManager.Instance.currentTattooGunLevel = coolnessLevel - 1;
        GameManager.Instance.UpgradeTattooGun();

        PlayerPrefs.SetInt("CoolnessUpgradeLevel", coolnessLevel);

        _levelText.SetText("Stage " + coolnessLevel);

        if (coolnessLevel == GameManager.Instance.GetTotalTattooGunAmount())
        {
            DisableCoolnessUpgradeButton();
        }
    }
}
