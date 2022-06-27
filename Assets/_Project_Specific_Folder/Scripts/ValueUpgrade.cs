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
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _levelText;
    private Button _button;
    private bool _isAdEnabled;

    private void Start()
    {
        _button = transform.GetComponent<Button>();
        _valueUpgradeButtonImage = transform.GetComponent<Image>();
        _costText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _levelText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        _levelText.SetText("$" + PlayerPrefs.GetInt("ValueUpgradeAmount", baseUpgradeAmount));
        
        CheckValueUpgradeButtonTypeStatus();

        if (!_isAdEnabled)
        {
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
            }
            else
            {
                _button.interactable = false;
            }   
        }
    }
    
    public void OnValueUpgradeButtonClick()
    {
        int currentUpgradeAmount = PlayerPrefs.GetInt("ValueUpgradeAmount", baseUpgradeAmount);
        int currentPriceTagScore = PlayerPrefs.GetInt("PriceTagBaseScore", 0);
        int priceTagTotalScore = currentPriceTagScore + currentUpgradeAmount;

        if (!_isAdEnabled)
        {
            if (StorageManager.GetTotalScore() >= requiredScoreForValueUpgrade)
            {
                StorageManager.Instance.SetCurrentScore(priceTagTotalScore);
                StorageManager.SetTotalScore(StorageManager.GetTotalScore() - requiredScoreForValueUpgrade);
                
                UiManager.Instance.UpdatePriceTag(priceTagTotalScore);
                UiManager.Instance.ValueUpgradeEffect(currentUpgradeAmount);
                UiManager.Instance.UpdateTotalScoreText(StorageManager.GetTotalScore());
                
                ValueUpgradeButtonEffects(priceTagTotalScore);
                CheckValueUpgradeButtonTypeStatus();
            }
            CheckValueUpgradeButtonAvailability();
            UiManager.Instance.coolnessUpgradeButton.GetComponent<CoolnessUpgrade>().CheckCoolnessUpgradeButtonAvailability();
        }
        else
        {
            Debug.Log("Ad Watched");
            
            StorageManager.Instance.SetCurrentScore(priceTagTotalScore);
            UiManager.Instance.UpdatePriceTag(priceTagTotalScore);
            UiManager.Instance.ValueUpgradeEffect(currentUpgradeAmount);

            ValueUpgradeButtonEffects(priceTagTotalScore);
            CheckValueUpgradeButtonTypeStatus();
        }
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
