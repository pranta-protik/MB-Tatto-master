using System;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private UpgradeDataSO upgradeData;
    [SerializeField] private GameObject lockedContainer;

    #region Init&Mono
    private void Awake()
    {
        upgradeData.UpgradeActivatedAction += OnActivate;
        upgradeData.UpgradeUnlockedAction += OnUnlocked;
    }

    private void Start()
    {
        gameObject.SetActive(upgradeData.IsAvailable);
        SetState(upgradeData.IsUnlocked);
    }

    private void OnDestroy()
    {
        upgradeData.UpgradeActivatedAction -= OnActivate;
        upgradeData.UpgradeUnlockedAction -= OnUnlocked;
    }
    #endregion

    #region Handlers
    private void OnActivate()
    {
        gameObject.SetActive(true);
    }

    private void OnUnlocked(UpgradeDataSO upgrade)
    {
        SetState(false);
    }
    #endregion
    
    #region Logic
    private void SetState(bool isLocked)
    {
        lockedContainer.SetActive(isLocked);
    }
    #endregion
}