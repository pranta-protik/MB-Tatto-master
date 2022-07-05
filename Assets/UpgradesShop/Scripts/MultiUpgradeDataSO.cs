using System;
using UnityEngine;

public abstract class MultiUpgradeDataSO : UpgradeDataSO
{
    [SerializeField] private int unlockPrice;
    
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

    public override void Unlock()
    {
        base.Unlock();
        
        UpgradesMaxedAction?.Invoke();
    }
    
    public override bool HasPurchasesAvailable()
    {
        return !IsUnlocked;
    }

    public override int GetNextPurchasePrice()
    {
        return unlockPrice;
    }
}