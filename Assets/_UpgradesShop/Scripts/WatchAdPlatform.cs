using HomaGames.HomaBelly;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdPlatform : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private float second;
    private float _time;
    private bool isProgressBarFilling;
    
    private const string PlayerTag = "Player";

    public void Init(bool isLocked)
    {
        if (isLocked)
        {
            return;
        }
        
        this.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        isProgressBarFilling = true;
        _time = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        if (isProgressBarFilling)
        {
            if (_time < second)
            {
                _time += Time.deltaTime;
                progressBar.fillAmount = _time / second;
            }
            else
            {
                isProgressBarFilling = false;
                WatchAd();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        isProgressBarFilling = false;
        progressBar.fillAmount = 0;
    }

    private void WatchAd()
    {
        // Subscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
            
        // Show Ad
        if (HomaBelly.Instance.IsRewardedVideoAdAvailable())
        {
            HomaBelly.Instance.ShowRewardedVideoAd(PlacementName.UNLOCK_STATION);
        }
        else
        {
            Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
        }
    }
    
    // Collect Ad Rewards
    private void OnRewardedVideoAdRewardedEvent(VideoAdReward obj)
    {
        transform.parent.GetComponent<AdUpgradeStation>().UnlockStation();
        this.gameObject.SetActive(false);
        
        // Rewarded Videos
        // Rewarded Claimed Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "taken" + ":" + PlacementName.UNLOCK_STATION);
        
        // Unsubscribe to Rewarded Video Ads
        Events.onRewardedVideoAdRewardedEvent -= OnRewardedVideoAdRewardedEvent;
    }
}