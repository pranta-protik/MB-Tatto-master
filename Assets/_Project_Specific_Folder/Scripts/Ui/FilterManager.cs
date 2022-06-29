using UnityEngine;

public class FilterManager : MonoBehaviour
{
    [SerializeField] private MobileScreen mobileScreen;
    public int currentFilterButtonId;
    private GameObject _captureButton;
    private GameObject _watchAdButton;

    private void Start()
    {
        _captureButton = mobileScreen.transform.GetChild(3).gameObject;
        _watchAdButton = mobileScreen.transform.GetChild(4).gameObject;
    }

    public void FilterEffect1(GameObject filterButtonObj)
    {
        NormalFilterEffect(filterButtonObj);
    }

    public void FilterEffect2(GameObject filterButtonObj)
    {
        NormalFilterEffect(filterButtonObj);
    }

    public void FilterEffect3(GameObject filterButtonObj)
    {
        WatchAdFilterEffect(filterButtonObj);
    }
    
    public void FilterEffect4(GameObject filterButtonObj)
    {
        WatchAdFilterEffect(filterButtonObj);
    }
    
    public void FilterEffect5(GameObject filterButtonObj)
    {
        WatchAdFilterEffect(filterButtonObj);
    }
    
    private void NormalFilterEffect(GameObject currentFilterObj)
    {
        SetCurrentFilterId(currentFilterObj);
        EnableCaptureButton();
    }

    private void WatchAdFilterEffect(GameObject currentFilterObj)
    {
        SetCurrentFilterId(currentFilterObj);
        
        if (PlayerPrefs.GetInt("FilterAdWatched" + currentFilterButtonId, 0) == 0)
        {
            EnableWatchAdButton();
        }
        else
        {
            EnableCaptureButton();
        }
    }
    
    private void SetCurrentFilterId(GameObject currentFilterObj)
    {
        FilterButton filterButton = currentFilterObj.GetComponent<FilterButton>();
        currentFilterButtonId = filterButton.buttonId;
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
