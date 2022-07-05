using DG.Tweening;
using HomaGames.HomaBelly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueUpgrade : MonoBehaviour
{
    [SerializeField] private Sprite normalValueUpgradeIcon;
    [SerializeField] private Sprite watchAdValueUpgradeIcon;
    [SerializeField] private int baseUpgradeAmount;
    [SerializeField] [Range(1, 5)] private int upgradeMultiplier;
    [SerializeField] private int requiredScoreForValueUpgrade;
    [SerializeField] private int startingLevelForUpgradeValueWatchingAd;
    
    private Image _valueUpgradeButtonImage;
    private GameObject _shineEffectObj;
    private TMP_Text _costText;
    private TMP_Text _levelText;
    private Button _button;
    private bool _isAdEnabled;
    private int _currentUpgradeAmount;
    private int _currentPriceTagScore;
    private int _priceTagTotalScore;
    private bool _isScaleEffectEnabled;

    private void Start()
    {
        _button = transform.GetComponent<Button>();
        _valueUpgradeButtonImage = transform.GetComponent<Image>();
        _shineEffectObj = transform.GetChild(2).gameObject;
        _costText = transform.GetChild(1).GetComponent<TMP_Text>();
        _levelText = transform.GetChild(0).GetComponent<TMP_Text>();
        
        _levelText.SetText("$" + PlayerPrefs.GetInt("ValueUpgradeAmount", baseUpgradeAmount));
        
        CheckValueUpgradeButtonTypeStatus();

        if (!_isAdEnabled)
        {
            _shineEffectObj.SetActive(false);
            CheckValueUpgradeButtonAvailability();    
        }
    }

    private void CheckValueUpgradeButtonTypeStatus()
    {
        if (PlayerPrefs.GetInt("ValueUpgradeLevel", 1) >= startingLevelForUpgradeValueWatchingAd)
        {
            _isAdEnabled = true;
            _valueUpgradeButtonImage.sprite = watchAdValueUpgradeIcon;
            _costText.gameObject.SetActive(false);
            
            if (!_isScaleEffectEnabled)
            {
                _isScaleEffectEnabled = true;
                _shineEffectObj.SetActive(true);
                transform.DOScale(new Vector3(1.45f, 1.45f, 1.45f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            
            // Rewarded Videos
            // Rewarded Suggested Event
            HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UPGRADE_VALUE);
        }
        else
        {
            _isAdEnabled = false;
            _valueUpgradeButtonImage.sprite = normalValueUpgradeIcon;
            _costText.gameObject.SetActive(true);
            _costText.SetText("$" + requiredScoreForValueUpgrade);
        }
    }
    
    public void CheckValueUpgradeButtonAvailability()
    {
        if (!_isAdEnabled)
        {
            if (StorageManager.GetTotalScore() >= requiredScoreForValueUpgrade)
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
    
    public void OnValueUpgradeButtonClick()
    {
        _currentUpgradeAmount = PlayerPrefs.GetInt("ValueUpgradeAmount", baseUpgradeAmount);
        _currentPriceTagScore = PlayerPrefs.GetInt("PriceTagBaseScore", 0);
        _priceTagTotalScore = _currentPriceTagScore + _currentUpgradeAmount;

        if (!_isAdEnabled)
        {
            if (StorageManager.GetTotalScore() >= requiredScoreForValueUpgrade)
            {
                StorageManager.Instance.SetCurrentScore(_priceTagTotalScore);
                StorageManager.SetTotalScore(StorageManager.GetTotalScore() - requiredScoreForValueUpgrade);
                
                UiManager.Instance.UpdatePriceTag(_priceTagTotalScore);
                UiManager.Instance.ValueUpgradeEffect(_currentUpgradeAmount);
                UiManager.Instance.UpdateTotalScoreText(StorageManager.GetTotalScore());
                
                ValueUpgradeButtonEffects(_priceTagTotalScore);
                CheckValueUpgradeButtonTypeStatus();
            }
            CheckValueUpgradeButtonAvailability();
            UiManager.Instance.coolnessUpgradeButton.GetComponent<CoolnessUpgrade>().CheckCoolnessUpgradeButtonAvailability();
        }
        else
        {
            // Subscribe to Rewarded Video Ads
            Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
            
            // Show Ad
            if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
            {
                HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.UPGRADE_VALUE);
            }
        }
    }

    // Collect Ad Rewards
    private void OnRewardedVideoAdRewardedEvent(VideoAdReward obj)
    {
        StorageManager.Instance.SetCurrentScore(_priceTagTotalScore);
        UiManager.Instance.UpdatePriceTag(_priceTagTotalScore);
        UiManager.Instance.ValueUpgradeEffect(_currentUpgradeAmount);

        ValueUpgradeButtonEffects(_priceTagTotalScore);
        CheckValueUpgradeButtonTypeStatus();
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.UPGRADE_VALUE);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }
    
    private void ValueUpgradeButtonEffects(int totalScore)
    {
        PlayerPrefs.SetInt("PriceTagBaseScore", totalScore);
        
        int lastUpgradeAmount = PlayerPrefs.GetInt("ValueUpgradeAmount", baseUpgradeAmount);
        int nextUpgradeAmount = lastUpgradeAmount * upgradeMultiplier;
            
        PlayerPrefs.SetInt("ValueUpgradeAmount", nextUpgradeAmount);
        _levelText.SetText("$" + nextUpgradeAmount);

        int lastUpgradeLevel = PlayerPrefs.GetInt("ValueUpgradeLevel", 1);
        PlayerPrefs.SetInt("ValueUpgradeLevel", lastUpgradeLevel + 1);
    }
}
