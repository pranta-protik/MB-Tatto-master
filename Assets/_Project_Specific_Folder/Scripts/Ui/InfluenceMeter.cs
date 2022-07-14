using System;
using System.Collections.Generic;
using DG.Tweening;
using GameAnalyticsSDK;
using HomaGames.HomaBelly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InfluenceMeterAttributes
{
    public float playerIconYPosition;
    public GameObject crossedOpponent;
}

public class InfluenceMeter : MonoBehaviour
{
    public List<InfluenceMeterAttributes> playerIconYPositions;
    public float playerIconMoveDuration;
    public float meterScrollDownThreshold;
    public float meterScrollDownAmount;
    public float meterScrollDownDuration;
    public float meterIconBaseYValue;
    public GameObject confettiEffect;
    
    private int _playerIconYPositionIndex;
    private int _yPositionIndex;
    private float _scrollDownThreshold;
    private int _targetCash;
    private RectTransform _playerIcon;
    private RectTransform _meterIcon;
    private TMP_Text _followersText;
    private TMP_Text _usernameText;
    private GameObject _cashBanner;
    private TMP_Text _cashText;
    private InfluencerStatus _influencerStatus;
    private GameObject _fightBanner;
    private GameObject _fightIcon;
    // private GameObject _nextButton;
    private GameObject _watchAdButton;
    private GameObject _foregroundScreen;
    private GameObject _skipButton;
    private GameObject _scoreMultiplierMeter;
    private GameObject _scoreMultiplierPointer;

    private void Start()
    {
        _fightBanner = transform.GetChild(4).gameObject;
        _fightIcon = transform.GetChild(5).gameObject;
        // _nextButton = transform.GetChild(6).gameObject;
        _watchAdButton = transform.GetChild(6).gameObject;
        _foregroundScreen = transform.GetChild(10).gameObject;
        _skipButton = transform.GetChild(7).gameObject;
        _scoreMultiplierMeter = transform.GetChild(8).gameObject;
        _scoreMultiplierPointer = transform.GetChild(9).gameObject;
        _cashBanner = transform.GetChild(2).gameObject;
        _cashText = transform.GetChild(3).GetComponent<TMP_Text>();
        
        _targetCash = StorageManager.Instance.currentLevelScore < 0 ? 500 : StorageManager.Instance.currentLevelScore;
        
        UpdateCash(_targetCash);

        Transform playerIconTransform = transform.GetChild(1).GetChild(0).GetChild(0);
        
        _followersText = playerIconTransform.GetChild(2).GetComponent<TMP_Text>();
        _followersText.SetText(UiManager.Instance.followerValue);

        _usernameText = playerIconTransform.GetChild(1).GetComponent<TMP_Text>();
        _usernameText.SetText(PlayerPrefs.GetString("Username"));
        
        _scrollDownThreshold = PlayerPrefs.GetFloat("ScrollDownThreshold", meterScrollDownThreshold);
        
        _playerIcon = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        _playerIconYPositionIndex = PlayerPrefs.GetInt("PlayerIconYPositionIndex", 0);

        if (_playerIconYPositionIndex < playerIconYPositions.Count)
        {
            _playerIcon.anchoredPosition = new Vector2(230f, playerIconYPositions[_playerIconYPositionIndex].playerIconYPosition);    
        }
        else
        {
            _playerIcon.anchoredPosition = new Vector2(230f, playerIconYPositions[playerIconYPositions.Count - 1].playerIconYPosition);
        }

        if (UiManager.Instance.isBadTattoo)
        {
            if (_playerIconYPositionIndex > 0)
            {
                _yPositionIndex = _playerIconYPositionIndex - 1;    
            }
            else
            {
                _yPositionIndex = _playerIconYPositionIndex;
            }
        }
        else
        {
            _yPositionIndex = _playerIconYPositionIndex + 1;
        }
        
        _meterIcon = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        _meterIcon.anchoredPosition = new Vector2(0f, PlayerPrefs.GetFloat("MeterIconYValue", meterIconBaseYValue));
        
        Invoke(nameof(UpdateInfluenceMeter), 0.5f);
    }

    public int GetCurrentLevelCash()
    {
        return _targetCash;
    }
    public void UpdateCash(int totalCash)
    {
        _cashText.SetText("$" + totalCash);
    }
    
    private void UpdateInfluenceMeter()
    {
        if (_yPositionIndex < playerIconYPositions.Count)
        {
            if (_playerIcon.anchoredPosition.y >= _scrollDownThreshold)
            {
                _meterIcon.DOAnchorPosY(_meterIcon.anchoredPosition.y - meterScrollDownAmount, meterScrollDownDuration).OnComplete(() =>
                {
                    PlayerPrefs.SetFloat("MeterIconYValue", _meterIcon.anchoredPosition.y);
                
                    _scrollDownThreshold += meterScrollDownThreshold;
                    PlayerPrefs.SetFloat("ScrollDownThreshold", _scrollDownThreshold);
                
                    UpdatePlayerIcon();
                });
            }
            else
            {
                UpdatePlayerIcon();
            }   
        }
        else
        {
            // EnableNextButton();
            EnableWatchAdButton();
        }
    }

    private void UpdatePlayerIcon()
    {
        _playerIcon.DOAnchorPosY(playerIconYPositions[_yPositionIndex].playerIconYPosition, playerIconMoveDuration).OnComplete(() =>
        {
            PlayerPrefs.SetInt("PlayerIconYPositionIndex", _yPositionIndex);
            
            if (playerIconYPositions[_yPositionIndex].crossedOpponent != null)
            {
                _influencerStatus = playerIconYPositions[_yPositionIndex].crossedOpponent.GetComponent<InfluencerStatus>();
                
                if (PlayerPrefs.GetInt("InfluencerStatus" + _influencerStatus.influencerId, 0) == 0)
                {
                    _fightBanner.transform.GetChild(1).gameObject.SetActive(false);
                    _fightBanner.transform.GetChild(2).gameObject.SetActive(false);
                 
                    _fightBanner.SetActive(true);
                    
                    _fightBanner.transform.GetChild(0).DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f).OnComplete(() =>
                    {
                        Invoke(nameof(StartWrestling), 0.7f);
                    });
                }
                else
                {
                    // EnableNextButton();
                    EnableWatchAdButton();
                }
            }
            else
            {
                // EnableNextButton();
                EnableWatchAdButton();
            }
        });   
    }

    public void OnWatchAdButtonClick()
    {
        // Subscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
        Events.onRewardedVideoAdClosedEvent += OnRewardedVideoAdClosedEvent;
                
        // Show Ad
        if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
        {
            HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.WRESTLE_OPPONENT);
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
        _fightBanner.SetActive(false);
        
        GameManager.Instance.isWrestling = true;
        PlayerPrefs.SetInt("InfluncerFightStatus" + _influencerStatus.influencerId, 1);
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.WRESTLE_OPPONENT);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }
    
    public void OnSkipButtonClick()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        
        _fightBanner.SetActive(false);
        _fightIcon.SetActive(false);
        UiManager.Instance.tapFastPanel.SetActive(false);
        
        PlayerPrefs.SetInt("InfluencerStatus" + _influencerStatus.influencerId, 1);
        PlayerPrefs.SetInt("InfluncerFightStatus" + _influencerStatus.influencerId, 0);
     
        // EnableNextButton();
        EnableWatchAdButton();
    }
    
    private void StartWrestling()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        _fightIcon.transform.GetChild(1).GetComponent<Image>().sprite = _influencerStatus.influencerIcon;
        _fightIcon.SetActive(true);
        
        GameManager.Instance.WrestlingSetup(_influencerStatus.influencerHandId);

        _fightBanner.transform.GetChild(1).gameObject.SetActive(true);
        _fightBanner.transform.GetChild(2).gameObject.SetActive(true);
            
        GameManager.Instance.isWrestling = false;
            
        // Rewarded Videos
        // Rewarded Suggested Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.WRESTLE_OPPONENT);
        
        // if (PlayerPrefs.GetInt("FirstFight", 1) == 1)
        // {
        //     _fightBanner.SetActive(false);
        //     GameManager.Instance.isWrestling = true;
        //     
        //     PlayerPrefs.SetInt("FirstFight", 0);
        //     PlayerPrefs.SetInt("InfluncerFightStatus" + _influencerStatus.influencerId, 1);
        // }
        // else
        // {
        //     _fightBanner.transform.GetChild(1).gameObject.SetActive(true);
        //     _fightBanner.transform.GetChild(2).gameObject.SetActive(true);
        //     
        //     GameManager.Instance.isWrestling = false;
        //     
        //     // Rewarded Videos
        //     // Rewarded Suggested Event
        //     HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.WRESTLE_OPPONENT);
        // }
    }
    
    public void CrossOpponentVisual()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        
        _fightIcon.SetActive(false);
        
        PlayerPrefs.SetInt("InfluencerStatus" + _influencerStatus.influencerId, 1);
        confettiEffect.SetActive(true);

        Transform followerDropTextTransform = _influencerStatus.transform.GetChild(2).GetChild(0);
        followerDropTextTransform.gameObject.SetActive(true);
        followerDropTextTransform.GetComponent<TMP_Text>().DOFade(0f, 0.3f).SetLoops(-1, LoopType.Restart);
        followerDropTextTransform.GetComponent<RectTransform>().DOAnchorPosY(130f, 0.3f).SetLoops(-1, LoopType.Restart);
        
        StartCoroutine(_influencerStatus.DecreaseFollowers());
        
        // Invoke(nameof(EnableNextButton), 2.5f);
        Invoke(nameof(EnableWatchAdButton), 2.5f);
    }

    // private void EnableNextButton()
    // {
    //     _nextButton.SetActive(true);
    //     _nextButton.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    // }
    
    private void EnableWatchAdButton()
    {
        _watchAdButton.SetActive(true);
        _watchAdButton.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        
        _skipButton.SetActive(true);
        
        _cashBanner.SetActive(false);
        _cashText.gameObject.SetActive(false);
        
        _scoreMultiplierMeter.SetActive(true);
        _scoreMultiplierPointer.SetActive(true);
        
        // Rewarded Videos
        // Rewarded Suggested Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.SCORE_MULTIPLIER);
    }

    public void DisplayCash()
    {
        _watchAdButton.SetActive(false);
        _skipButton.SetActive(false);
        
        _cashBanner.SetActive(true);
        _cashText.gameObject.SetActive(true);
        
        _scoreMultiplierMeter.SetActive(false);
        _scoreMultiplierPointer.SetActive(false);
    }
    
    public void EnableUnlockScreen()
    {
         GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Completed");
        _foregroundScreen.SetActive(true);
        _foregroundScreen.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            UiManager.Instance.unlockPanel.SetActive(true);
            gameObject.SetActive(false); 
        });
    }
}
