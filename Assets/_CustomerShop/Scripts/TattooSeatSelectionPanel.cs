using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HomaGames.HomaBelly;
using UnityEngine;
using UnityEngine.UI;

public class TattooSeatSelectionPanel : MonoBehaviour
{
    public Action<int> TattooSeatUnlockAction;
        
    [SerializeField] private RectTransform selectionPanel;
    [SerializeField] private float targetYAnchorPosition;
    
    private Image _backgroundImage;
    private int _selectedSeatId;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
    }

    public void ShowPanel()
    {
        selectionPanel.DOAnchorPosY(targetYAnchorPosition, 1f);
        _backgroundImage.enabled = true;
        
        // Rewarded Videos
        // Rewarded Suggested Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UNLOCK_TATTOO_SEAT);
    }
    
    private void HidePanel()
    {
        selectionPanel.DOAnchorPosY(-targetYAnchorPosition, 1f);
        _backgroundImage.enabled = false;
    }

    public void OnTattooSeatSelectionButtonClick(int id)
    {
        _selectedSeatId = id;
        
        TattooSeatUnlockAction?.Invoke(_selectedSeatId);
        HidePanel();
    }

    public void OnTattooSeatSelectionAdButtonClick(int id)
    {
        _selectedSeatId = id;
        
        // Subscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
        Events.onRewardedVideoAdClosedEvent += OnRewardedVideoAdClosedEvent;

        // Show Ad
        if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
        {
            HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.UNLOCK_TATTOO_SEAT);
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
        TattooSeatUnlockAction?.Invoke(_selectedSeatId);
        HidePanel();
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.UNLOCK_TATTOO_SEAT);

        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }
    
}
