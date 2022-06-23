using System;
using System.Collections.Generic;
using DG.Tweening;
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
    private TextMeshProUGUI _followersText;
    private TextMeshProUGUI _cashText;
    private InfluencerStatus _influencerStatus;

    private void Start()
    {
        _targetCash = StorageManager.Instance.currentLevelScore < 0 ? 500 : StorageManager.Instance.currentLevelScore;
        _cashText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _cashText.SetText("$" + _targetCash);
        
        StorageManager.SetTotalScore(StorageManager.GetTotalScore() + _targetCash);

        _followersText = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _followersText.SetText(UiManager.Instance.followerValue);

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
            EnableNextButton();
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
                    transform.GetChild(4).gameObject.SetActive(true);
                    transform.GetChild(4).DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f).OnComplete(() =>
                    {
                        Invoke(nameof(StartWrestling), 0.7f);
                    });
                }
                else
                {
                    EnableNextButton();
                }
            }
            else
            {
                EnableNextButton();
            }
        });   
    }

    private void StartWrestling()
    {
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        GameManager.Instance.WrestlingSetup(_influencerStatus.influencerHandId);   
    }
    
    public void CrossOpponentVisual()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        
        PlayerPrefs.SetInt("InfluencerStatus" + _influencerStatus.influencerId, 1);
        confettiEffect.SetActive(true);
        _influencerStatus.transform.GetChild(0).DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetDelay(0.5f).OnComplete(EnableNextButton);
    }
    
    private void EnableNextButton()
    {
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(5).DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void EnableUnlockScreen()
    {
        transform.GetChild(6).gameObject.SetActive(true);
        transform.GetChild(6).GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            UiManager.Instance.unlockPanel.SetActive(true);
            gameObject.SetActive(false); 
        });
    }
}
