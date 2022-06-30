using System;
using UnityEngine;

public abstract class MultiUpgradeDataSO : UpgradeDataSO
{
    [SerializeField] private int unlockPrice;
    
    protected bool isAvailable;
    protected bool isUnlocked;
    
    private string availableKey;
    private string unlockedKey;
    
    public bool IsAvailable => isAvailable;
    public bool IsUnlocked => isUnlocked;

    public Action UpgradeActivatedAction;
    public Action<MultiUpgradeDataSO> UpgradeUnlockedAction;

    protected override void Awake()
    {
        base.Awake();
        
        availableKey = string.Concat("AvailableKey", "_", upgradeType, "_", upgradeName);
        unlockedKey = string.Concat("UnlockedKey", "_", upgradeType, "_", upgradeName);
        
        isAvailable = PlayerPrefs.GetInt(availableKey, 0) == 1;
        isUnlocked = PlayerPrefs.GetInt(unlockedKey, 0) == 1;
    }

    public override void Deposit(int amount)
    {
        CurrencyDeposited += amount;

        if(CurrencyDeposited >= unlockPrice)
        {
            Unlock();

            if(CurrencyDeposited > unlockPrice)
            {
                Debug.LogError("[UPGRADES] To much money deposited for upgrade " + upgradeName);
            }
        }
    }

    public void Activate()
    {
        if(isAvailable)
        {
            Debug.LogError(string.Concat("[UPGRADES] Upgrade ", upgradeName, " is already active"));
            return;
        }
        
        isAvailable = true;
        PlayerPrefs.SetInt(availableKey, 1);
        UpgradeActivatedAction?.Invoke();
    }

    public void Unlock()
    {
        if(isUnlocked)
        {
            Debug.LogError(string.Concat("[UPGRADES] Upgrade ", upgradeName, " is already unlocked"));
            return;
        }
        
        isUnlocked = true;
        PlayerPrefs.SetInt(unlockedKey, 1);
        UpgradeUnlockedAction?.Invoke(this);
        UpgradesMaxedAction?.Invoke();
    }
}