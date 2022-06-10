using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfluenceMeter : MonoBehaviour
{
    public float playerIconMoveDistance;
    public float playerIconMoveDuration;
    public float playerIconBaseYValue;
    public float playerIconMaxYValue;
    
    public float meterScrollDownThreshold;
    public float meterScrollDownAmount;
    public float meterScrollDownDuration;
    public float meterIconBaseYValue;

    private float _scrollDownThreshold;
    private RectTransform _playerIcon;
    private RectTransform _meterIcon;
    private TextMeshProUGUI _followersText;
    private TextMeshProUGUI _cashText;

    private float _currentCash;
    private float _startCash;
    private int _targetCash;
    private bool _shouldUpdateCashText;
    
    private void Start()
    {
        _cashText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _cashText.SetText("$0");
        
        _followersText = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _followersText.SetText(PlayerPrefs.GetInt("LastFollowers", 0).ToString());

        _scrollDownThreshold = PlayerPrefs.GetFloat("ScrollDownThreshold", meterScrollDownThreshold);
        
        _playerIcon = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        _playerIcon.anchoredPosition = new Vector2(230f, PlayerPrefs.GetFloat("PlayerIconYValue", playerIconBaseYValue));
        
        _meterIcon = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        _meterIcon.anchoredPosition = new Vector2(0f, PlayerPrefs.GetFloat("MeterIconYValue", meterIconBaseYValue));
        
        _targetCash = StorageManager.Instance.RewardValue < 0 ? 500 : StorageManager.Instance.RewardValue;
        _currentCash = 0;
        _startCash = _currentCash;
        _shouldUpdateCashText = true;
    }

    private void Update()
    {
        if (_shouldUpdateCashText)
        {
            UpdateCashText();
        }
    }

    private void UpdateInfluenceMeter()
    {
        if (_playerIcon.anchoredPosition.y < playerIconMaxYValue)
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
    }

    private void UpdatePlayerIcon()
    {
        _playerIcon.DOAnchorPosY(_playerIcon.anchoredPosition.y + playerIconMoveDistance, playerIconMoveDuration).OnComplete(() =>
        {
            PlayerPrefs.SetFloat("PlayerIconYValue", _playerIcon.anchoredPosition.y);
            Invoke(nameof(EnableUnlockScreen), 2f);
        });   
    }

    private void EnableUnlockScreen()
    {
        transform.GetChild(4).GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            UiManager.Instance.UnlockPanel.SetActive(true);
            gameObject.SetActive(false); 
        });
    }
    
    private void UpdateCashText()
    {
        if (_currentCash < _targetCash)
        {
            _currentCash += ((_targetCash - _startCash) / 1.5f) * Time.deltaTime;
            _currentCash = Mathf.Clamp(_currentCash, 0, _targetCash);
        }
        else
        {
            _shouldUpdateCashText = false;
            StorageManager.SaveTotalCoin(StorageManager.GetTotalCoin() + _targetCash);
            UpdateInfluenceMeter();
        }
        _cashText.SetText("$" + Mathf.RoundToInt(_currentCash));
    }
}
