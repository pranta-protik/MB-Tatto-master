using System;
using System.Collections.Generic;
using DG.Tweening;
using HomaGames.HomaBelly;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    [Serializable]
    public struct Multiplier
    {
        public int value;
        public float startAngle;
        public float endAngle;
    }
    
    [SerializeField] private GameObject multiplierPointer;
    [SerializeField] private List<Multiplier> multiplierValues;

    private void Start()
    {
        multiplierPointer.transform.DORotate(new Vector3(0f, 0f, 140f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private Vector3 _pointerAngle;
    public void OnWatchAdButtonClick()
    {
        _pointerAngle = multiplierPointer.transform.localEulerAngles;
        
        // Subscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
                
        // Show Ad
        if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
        {
            HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.SCORE_MULTIPLIER);
        }
    }

    private void OnRewardedVideoAdRewardedEvent(VideoAdReward obj)
    {
        multiplierPointer.transform.DOKill();
        multiplierPointer.transform.localEulerAngles = _pointerAngle;
        
        foreach (Multiplier multiplierValue in multiplierValues)
        {
            if (_pointerAngle.z >= multiplierValue.startAngle && _pointerAngle.z <= multiplierValue.endAngle)
            {
                Debug.Log(multiplierValue.value);
                break;
            }
        }
        
        Invoke(nameof(EnableUnlockScreen), 2f);
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.SCORE_MULTIPLIER);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent; 
    }

    private void EnableUnlockScreen()
    {
        transform.parent.GetComponent<InfluenceMeter>().EnableUnlockScreen();
    }
}
