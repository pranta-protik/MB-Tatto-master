using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfluenceMeter : MonoBehaviour
{
    public List<float> playerIconYPositions;
    public float playerIconMoveDuration;

    public float meterScrollDownThreshold;
    public float meterScrollDownAmount;
    public float meterScrollDownDuration;
    public float meterIconBaseYValue;

    private int _playerIconYPositionIndex;
    private float _scrollDownThreshold;
    private RectTransform _playerIcon;
    private RectTransform _meterIcon;
    private TextMeshProUGUI _followersText;
    private TextMeshProUGUI _cashText;
    
    private int _targetCash;

    private void Start()
    {
        _targetCash = StorageManager.Instance.RewardValue < 0 ? 500 : StorageManager.Instance.RewardValue;
        _cashText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _cashText.SetText("$" + _targetCash);
        
        StorageManager.SaveTotalCoin(StorageManager.GetTotalCoin() + _targetCash);

        _followersText = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _followersText.SetText(UiManager.Instance.followerValue);

        _scrollDownThreshold = PlayerPrefs.GetFloat("ScrollDownThreshold", meterScrollDownThreshold);
        
        _playerIcon = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        _playerIconYPositionIndex = PlayerPrefs.GetInt("PlayerIconYPositionIndex", 0);
        if (_playerIconYPositionIndex < playerIconYPositions.Count)
        {
            _playerIcon.anchoredPosition = new Vector2(230f, playerIconYPositions[_playerIconYPositionIndex]);    
        }
        else
        {
            _playerIcon.anchoredPosition = new Vector2(230f, playerIconYPositions[playerIconYPositions.Count - 1]);
        }

        _meterIcon = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        _meterIcon.anchoredPosition = new Vector2(0f, PlayerPrefs.GetFloat("MeterIconYValue", meterIconBaseYValue));
        
        Invoke(nameof(UpdateInfluenceMeter), 0.5f);
    }

    private void UpdateInfluenceMeter()
    {
        if (_playerIconYPositionIndex < playerIconYPositions.Count-1)
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
        _playerIcon.DOAnchorPosY(playerIconYPositions[_playerIconYPositionIndex + 1], playerIconMoveDuration).OnComplete(() =>
        {
            PlayerPrefs.SetInt("PlayerIconYPositionIndex", _playerIconYPositionIndex + 1);
            EnableNextButton();
        });   
    }

    private void EnableNextButton()
    {
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(4).DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void EnableUnlockScreen()
    {
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(5).GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            UiManager.Instance.UnlockPanel.SetActive(true);
            gameObject.SetActive(false); 
        });
    }
}
