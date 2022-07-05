using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] protected UpgradeDataSO upgradeData;
    [SerializeField] private GameObject lockedContainer;
    [SerializeField] private PayPlatform payPlatform;

    #region Init&Mono
    protected virtual void Awake()
    {
        upgradeData.UpgradeActivatedAction += OnActivate;
        upgradeData.UpgradeUnlockedAction += OnUnlocked;
        upgradeData.UpgradesMaxedAction += OnUpgradesMaxed;
    }
    
    private void Start()
    {
        payPlatform.Init(upgradeData);
        
        //TEMP disabled
        // gameObject.SetActive(upgradeData.IsAvailable);
        SetState(upgradeData.IsUnlocked);
    }

    protected virtual void OnDestroy()
    {
        upgradeData.UpgradeActivatedAction -= OnActivate;
        upgradeData.UpgradeUnlockedAction -= OnUnlocked;
        upgradeData.UpgradesMaxedAction -= OnUpgradesMaxed;
    }
    #endregion

    #region Handlers
    private void OnActivate()
    {
        gameObject.SetActive(true);
    }

    protected virtual void OnUnlocked(UpgradeDataSO upgrade)
    {
        SetState(false);
    }

    private void OnUpgradesMaxed()
    {

    }
    #endregion

    #region Logic
    private void SetState(bool isLocked)
    {
        lockedContainer.SetActive(isLocked);
    }

    #endregion
}