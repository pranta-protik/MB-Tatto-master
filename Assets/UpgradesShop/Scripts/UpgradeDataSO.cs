using System;
using System.Collections;
using UnityEngine;

public abstract class UpgradeDataSO : ScriptableObject
{
    public string upgradeName;

    protected bool isAvailable;
    protected bool isUnlocked;
    protected string availableKey;
    protected string unlockedKey;

    public bool IsAvailable => isAvailable;
    public bool IsUnlocked => isUnlocked;

    public Action UpgradeActivatedAction;
    public Action<UpgradeDataSO> UpgradeUnlockedAction;

    [HideInInspector] public UpgradeType upgradeType;

    private int currencyDeposited;
    private string currencyDepositedKey;

    public int CurrencyDeposited
    {
        get => currencyDeposited;
        set
        {
            currencyDeposited = value;
            PlayerPrefs.SetInt(currencyDepositedKey, currencyDeposited);
        }
    }

    public Action UpgradesMaxedAction;

    public abstract void Deposit(int amount);
    public abstract bool HasPurchasesAvailable();

    protected virtual void Awake()
    {
        availableKey = string.Concat("AvailableKey", "_", upgradeType, "_", upgradeName);
        unlockedKey = string.Concat("UnlockedKey", "_", upgradeType, "_", upgradeName);

        isAvailable = PlayerPrefs.GetInt(availableKey, 0) == 1;
        isUnlocked = PlayerPrefs.GetInt(unlockedKey, 0) == 1;

        currencyDepositedKey = string.Concat("CurrencyDepositedKey", "_", upgradeType, "_", upgradeName);
        currencyDeposited = PlayerPrefs.GetInt(currencyDepositedKey, 0);
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

[Serializable]
public enum UpgradeType
{
    TattooMachine = 0,
    TattooDesign = 1,
    Jewelry = 2
}