using System;
using System.Collections;
using UnityEngine;

public abstract class UpgradeDataSO : ScriptableObject
{
    public string upgradeName;

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
    
    protected virtual void Awake()
    {
        currencyDepositedKey = string.Concat("CurrencyDepositedKey", "_", upgradeType, "_", upgradeName);
        currencyDeposited = PlayerPrefs.GetInt(currencyDepositedKey, 0);
    }
}

[Serializable]
public enum UpgradeType
{
    TattooMachine = 0,
    TattooDesign = 1,
    Jewelry = 2
}