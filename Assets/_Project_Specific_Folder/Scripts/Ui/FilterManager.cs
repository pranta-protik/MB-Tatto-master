using HomaGames.HomaBelly;
using UnityEngine;

public class FilterManager : MonoBehaviour
{
    [SerializeField] private MobileScreen mobileScreen;
    public int currentFilterButtonId;
    
    private GameObject _captureButton;
    private GameObject _watchAdButton;
    private FilterButton _filterButton;

    private void Start()
    {
        _captureButton = mobileScreen.transform.GetChild(3).gameObject;
        _watchAdButton = mobileScreen.transform.GetChild(4).gameObject;
    }

    public void ResetFilter()
    {
        EnableCaptureButton();
    }

    public void SetFilter(GameObject filterButtonObj)
    {
        SetCurrentFilterId(filterButtonObj);

        if (_filterButton.isWatchAdRequired)
        {
            WatchAdFilterButton();
        }
        else
        {
            NormalFilterButton();
        }
    }

    private void SetCurrentFilterId(GameObject currentFilterObj)
    {
        _filterButton = currentFilterObj.GetComponent<FilterButton>();
        currentFilterButtonId = _filterButton.buttonId;
    }
    
    private void NormalFilterButton()
    {
        EnableCaptureButton();
    }
    
    private void WatchAdFilterButton()
    {
        if (PlayerPrefs.GetInt("FilterAdWatched" + currentFilterButtonId, 0) == 0)
        {
            EnableWatchAdButton();
            
            // Rewarded Videos
            // Rewarded Suggested Event
            HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UnlockFilter);
        }
        else
        {
            EnableCaptureButton();
        }
    }
    
    private void EnableCaptureButton()
    {
        _captureButton.SetActive(true);
        _watchAdButton.SetActive(false);    
    }

    private void EnableWatchAdButton()
    {
        _captureButton.SetActive(false);
        _watchAdButton.SetActive(true);
    }
}
