using HomaGames.HomaBelly;
using UnityEngine;

public class PoseManager : MonoBehaviour
{
    [SerializeField] private int _currentPoseButtonId;
    private PoseButton _poseButton;
    
    private void WatchAd()
    {
        // Subscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
        Events.onRewardedVideoAdClosedEvent += OnRewardedVideoAdClosedEvent;
            
        // Show Ad
        if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
        {
            HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.UNLOCK_POSE);   
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
        PlayerPrefs.SetInt("PoseAdWatched" + _currentPoseButtonId, 1);
        _poseButton.buttonImage.sprite = _poseButton.normalIcon; 
        GameManager.Instance.PlayPoseAnimation(_poseButton.animationId);
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.UNLOCK_POSE);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }

    public void ResetPose()
    {
        GameManager.Instance.ResetPose();
    }

    public void SetPose(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        
        if (_poseButton.isWatchAdRequired)
        {
            WatchAdPoseButton(_poseButton.animationId);
        }
        else
        {
            NormalPoseButton(_poseButton.animationId);
        }
    }

    private void SetCurrentPoseButtonId(GameObject currentPoseButtonObj)
    {
        _poseButton = currentPoseButtonObj.GetComponent<PoseButton>();
        _currentPoseButtonId = _poseButton.buttonId;
    }

    private void NormalPoseButton(int animationIndex)
    {
        GameManager.Instance.PlayPoseAnimation(animationIndex);
    }
    
    private void WatchAdPoseButton(int animationIndex)
    {
        if (PlayerPrefs.GetInt("PoseAdWatched" + _currentPoseButtonId, 0) == 0)
        {
            WatchAd();
        }
        else
        {
            GameManager.Instance.PlayPoseAnimation(animationIndex);
        }
    }
}
