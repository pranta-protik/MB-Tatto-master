using DG.Tweening;
using HomaGames.HomaBelly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoolnessUpgrade : MonoBehaviour
{
    [SerializeField] private Sprite normalCoolnessUpgradeIcon;
    [SerializeField] private Sprite watchAdCoolnessUpgradeIcon;
    [SerializeField] private Sprite maxedOutCoolnessUpgradeIcon;
    [SerializeField] private int requiredScoreForCoolnessUpgrade;
    [SerializeField] private int startingLevelForUpgradeCoolnessWatchingAd;

    private Image _coolnessUpgradeButtonImage;
    private Button _button;
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _levelText;
    private bool _isAdEnabled;
    private bool _isMaxedOut;
    private int _currentCoolnessLevel;

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
        
        if (PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1) == GameManager.Instance.GetTotalTattooGunAmount() * 2)
        {
            _isMaxedOut = true;
            DisableCoolnessUpgradeButton();
        }
    }

    private void CheckCoolnessUpgradeButtonTypeStatus()
    {
        if (!_isMaxedOut)
        {
            if (PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1) >= startingLevelForUpgradeCoolnessWatchingAd)
            {
                _isAdEnabled = true;
                _coolnessUpgradeButtonImage.sprite = watchAdCoolnessUpgradeIcon;
                _costText.gameObject.SetActive(false);
                
                // Rewarded Videos
                // Rewarded Suggested Event
                HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UpgradeCoolness);
            }
            else
            {
                _isAdEnabled = false;
                _coolnessUpgradeButtonImage.sprite = normalCoolnessUpgradeIcon;
                _costText.gameObject.SetActive(true);
                _costText.SetText("$" + requiredScoreForCoolnessUpgrade);
            }    
        }
    }

    public void CheckCoolnessUpgradeButtonAvailability()
    {
        if (!_isAdEnabled)
        {
            if (StorageManager.GetTotalScore() >= requiredScoreForCoolnessUpgrade)
            {
                _button.interactable = true;
                _button.image.DOFade(1f, 0.1f);
                _button.transform.GetChild(0).GetComponent<TMP_Text>().DOFade(1f, 0.1f);
                _button.transform.GetChild(1).GetComponent<TMP_Text>().DOFade(1f, 0.1f);
            }
            else
            {
                _button.interactable = false;
                _button.image.DOFade(0.5f, 0.1f);
                _button.transform.GetChild(0).GetComponent<TMP_Text>().DOFade(0.3f, 0.1f);
                _button.transform.GetChild(1).GetComponent<TMP_Text>().DOFade(0.3f, 0.1f);
            }
        }
    }

    private void DisableCoolnessUpgradeButton()
    {
        _coolnessUpgradeButtonImage.sprite = maxedOutCoolnessUpgradeIcon;
        _levelText.gameObject.SetActive(false);
        _button.interactable = false;
    }
    
    public void OnCoolnessUpgradeButtonClick()
    {
        _currentCoolnessLevel = PlayerPrefs.GetInt("CoolnessUpgradeLevel", 1);

        if (_currentCoolnessLevel < GameManager.Instance.GetTotalTattooGunAmount() * 2)
        {
            _currentCoolnessLevel += 1;

            if (!_isAdEnabled)
            {
                if (StorageManager.GetTotalScore() >= requiredScoreForCoolnessUpgrade)
                {
                    StorageManager.SetTotalScore(StorageManager.GetTotalScore() - requiredScoreForCoolnessUpgrade);
                    
                    UiManager.Instance.UpdateTotalScoreText(StorageManager.GetTotalScore());
                    
                    CoolnessUpgradeButtonEffects(_currentCoolnessLevel);
                    CheckCoolnessUpgradeButtonTypeStatus();
                }
                CheckCoolnessUpgradeButtonAvailability();
                UiManager.Instance.valueUpgradeButton.GetComponent<ValueUpgrade>().CheckValueUpgradeButtonAvailability();
            }
            else
            {
                // Subscribe to Rewarded Video Ads
                Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
                
                // Show Ad
                if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
                {
                    HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.UpgradeCoolness);
                }
            }
        }
    }

    // Collect Ad Rewards
    private void OnRewardedVideoAdRewardedEvent(VideoAdReward obj)
    {
        CoolnessUpgradeButtonEffects(_currentCoolnessLevel);
        CheckCoolnessUpgradeButtonTypeStatus();
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.UpgradeCoolness);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }
    
    private void CoolnessUpgradeButtonEffects(int coolnessLevel)
    {
        if (coolnessLevel <= GameManager.Instance.GetTotalTattooGunAmount())
        {
            GameManager.Instance.currentTattooGunLevel = coolnessLevel - 1;
        }
        else
        {
            GameManager.Instance.currentTattooGunLevel = (coolnessLevel - 1) % GameManager.Instance.GetTotalTattooGunAmount();
            GameManager.Instance.isGoldenTattooGunActivated = true;
        }
        
        GameManager.Instance.UpgradeTattooGun();

        PlayerPrefs.SetInt("CoolnessUpgradeLevel", coolnessLevel);

        _levelText.SetText("Stage " + coolnessLevel);

        if (coolnessLevel == GameManager.Instance.GetTotalTattooGunAmount() * 2)
        {
            _isMaxedOut = true;
            DisableCoolnessUpgradeButton();
        }
    }
}
