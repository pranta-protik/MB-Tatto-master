using UnityEngine;
using UnityEngine.UI;

public class WatchAdPlatform : MonoBehaviour
{
    private float _time;
    private bool isProgressBarFilling;
    [SerializeField] private Image progressBar;
    [SerializeField] private float second;
    
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
        transform.parent.GetComponent<AdUpgradeStation>().UnlockStation();
        this.gameObject.SetActive(false);
    }
}