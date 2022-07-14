using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HomaGames.HomaBelly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectionMenu : MonoBehaviour
{
    [SerializeField] private GameObject handCardPrefab;
    [SerializeField] private List<Sprite> handCardTextures = new List<Sprite>();
    public List<GameObject> handCards = new List<GameObject>();
    [SerializeField] private Sprite normalUnlockButtonIcon;
    [SerializeField] private Sprite watchAdUnlockButtonIcon;
    [SerializeField] private int startingAmountForUnlockHandWatchingAd;
    [SerializeField] private int baseUnlockCost;
    [SerializeField] [Range(1, 5)] private int multiplier;
    
    private Image _unlockButtonImage;
    private TextMeshProUGUI _costText;
    private GameObject _unlockButton;
    private int _totalCards;
    private bool _isAdEnabled;
    
    private void Awake()
    {
        _unlockButton = transform.GetChild(3).gameObject;
        _unlockButtonImage = _unlockButton.GetComponent<Image>();
        _costText = _unlockButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Vector3 spawnPosition = transform.GetChild(2).GetChild(0).position;
        Vector2 anchoredSpawnPosition = transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchoredPosition;

        _totalCards = handCardTextures.Count;
        
        for (int i = 0; i < _totalCards; i++)
        {
            GameObject handCard = Instantiate(handCardPrefab, spawnPosition, Quaternion.identity, transform.GetChild(2));

            handCard.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(anchoredSpawnPosition.x + ((i % 3) * 270), anchoredSpawnPosition.y - ((i / 3) * 230));
            
            handCard.transform.GetChild(1).GetComponent<Image>().sprite = handCardTextures[i];
            handCard.GetComponent<HandCard>().handId = i;
            
            handCards.Add(handCard);
        }
    }

    public void CheckUnlockButtonTypeStatus()
    {
        if (PlayerPrefs.GetInt("UnlockAmount", 1) >= startingAmountForUnlockHandWatchingAd)
        {
            _isAdEnabled = true;
            _unlockButtonImage.sprite = watchAdUnlockButtonIcon;
            _costText.gameObject.SetActive(false);
            
            // Rewarded Videos
            // Rewarded Suggested Event
            HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UNLOCK_NEW_HAND);
        }
        else
        {
            _isAdEnabled = false;
            _unlockButtonImage.sprite = normalUnlockButtonIcon;
            _costText.gameObject.SetActive(true);
            _costText.SetText("$" + PlayerPrefs.GetInt("UnlockCost", baseUnlockCost));
        }
    }
    
    public void CheckUnlockButtonAvailability()
    {
        if (!_isAdEnabled)
        {
            if (StorageManager.GetTotalScore() >= PlayerPrefs.GetInt("UnlockCost", baseUnlockCost))
            {
                EnableButton();
            }
            else
            {
                DisableButton();
            }    
        }
    }

    private void DisableButton()
    {
        Button button = _unlockButton.GetComponent<Button>();
        button.interactable = false;
        button.image.DOFade(0.5f, 0.1f);
        _unlockButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0.3f, 0.1f);
    }

    private void EnableButton()
    {
        Button button = _unlockButton.GetComponent<Button>();
        button.interactable = true;
        button.image.DOFade(1f, 0.1f);
        _unlockButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1f, 0.1f);
    }

    private void HideButton()
    {
        _unlockButton.SetActive(false);
    }

    private void ShowButton()
    {
        _unlockButton.SetActive(true);
    }

    public void OnUnlockButtonClick()
    {
        if (!_isAdEnabled)
        {
            int currentUnlockCost = PlayerPrefs.GetInt("UnlockCost", baseUnlockCost);
            
            if (StorageManager.GetTotalScore() >= currentUnlockCost)
            {
                StorageManager.SetTotalScore(StorageManager.GetTotalScore() - currentUnlockCost);
                UiManager.Instance.UpdateTotalScoreText(StorageManager.GetTotalScore());
                
                int nextUnlockCost = currentUnlockCost * multiplier;
                PlayerPrefs.SetInt("UnlockCost", nextUnlockCost);

                UnlockButtonEffects();
                CheckUnlockButtonTypeStatus();
            }
            CheckUnlockButtonAvailability();
            UiManager.Instance.valueUpgradeButton.GetComponent<ValueUpgrade>().CheckValueUpgradeButtonAvailability();
            UiManager.Instance.coolnessUpgradeButton.GetComponent<CoolnessUpgrade>().CheckCoolnessUpgradeButtonAvailability();
        }
        else
        {
            // Subscribe to Rewarded Video Ads
            Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
            Events.onRewardedVideoAdClosedEvent += OnRewardedVideoAdClosedEvent;
            
            // Show Ad
            if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
            {
                HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.UNLOCK_NEW_HAND);   
            }
        }
    }

    private void OnRewardedVideoAdClosedEvent(string obj)
    {
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
        Events.onRewardedVideoAdClosedEvent -= OnRewardedVideoAdClosedEvent;
    }
    
    // Collect Ad Rewards
    private void OnRewardedVideoAdRewardedEvent(VideoAdReward obj)
    {
        UnlockButtonEffects();
        CheckUnlockButtonTypeStatus();
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.UNLOCK_NEW_HAND);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }
    
    private void UnlockButtonEffects()
    {
        int currentUnlockAmount = PlayerPrefs.GetInt("UnlockAmount", 1);
        PlayerPrefs.SetInt("UnlockAmount", currentUnlockAmount + 1);
        
        List<int> randomHandIds = new List<int>();

        foreach (GameObject card in handCards)
        {
            if (card.GetComponent<HandCard>().unlockStatus == 0)
            {
                randomHandIds.Add(card.GetComponent<HandCard>().handId);
            }

            card.GetComponent<HandCard>().selectionImage.SetActive(false);
        }
        HideButton();
        StartCoroutine(RandomizeVisualEffect(randomHandIds));
    }

    IEnumerator RandomizeVisualEffect(List<int> randomHandIds)
    {
        HandCard handCard = null;

        int i = 0;

        while (i < 10)
        {
            int randomId = Random.Range(0, randomHandIds.Count);

            int randomUnlockId = randomHandIds[randomId];

            handCard = handCards[randomUnlockId].GetComponent<HandCard>();
            handCard.selectionImage.SetActive(true);

            yield return new WaitForSeconds(0.2f);

            handCard.selectionImage.SetActive(false);
            i++;
        }

        if (handCard != null)
        {
            handCard.UnlockHandCard(handCard.handId);
        }

        if (randomHandIds.Count <= 1)
        {
            HideButton();
        }
        else
        {
            ShowButton();    
        }
    }
}
