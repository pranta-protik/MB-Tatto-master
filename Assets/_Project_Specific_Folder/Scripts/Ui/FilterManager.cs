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
            WatchAdFilterEffect();
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
    
    private void WatchAdFilterEffect()
    {
        if (PlayerPrefs.GetInt("FilterAdWatched" + currentFilterButtonId, 0) == 0)
        {
            EnableWatchAdButton();
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
