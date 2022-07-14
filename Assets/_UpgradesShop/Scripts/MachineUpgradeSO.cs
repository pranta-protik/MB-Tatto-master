using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Upgrades/MachineUpgradeData")]
public class MachineUpgradeSO : UpgradeDataSO
{
    [FormerlySerializedAs("machineData")] [FormerlySerializedAs("machineAssets")] public List<MachineData> machineDatas;
    
    private int unlockedLevel;
    private int currentPrice;
    
    public int maxLevel => machineDatas.Count;
    public int UnlockedLevel => unlockedLevel;
    public MachineData CurrentMachineData => machineDatas[unlockedLevel];

    public GameObject defaultMachine;

    public Action<int> LevelChangedAction;
    
    private const string LEVEL_KEY = "LevelKey";

    public override void Init()
    {
        upgradeType = UpgradeType.TattooMachine;
        
        base.Init();
        
        isAvailable = true;
        isUnlocked = true;
        unlockedLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public override void Deposit(int amount)
    {
        CurrencyDeposited += amount;
        currentPrice = machineDatas[unlockedLevel].upgradePrize;

        if(CurrencyDeposited >= currentPrice)
        {
            if(CurrencyDeposited > currentPrice && unlockedLevel == maxLevel)
            {
                Debug.LogError("[UPGRADES] To much money deposited for machine upgrade");
            }
            
            CurrencyDeposited = 0;
            UpgradeMachine();
            PaymentSuccessfulAction?.Invoke();
        }
    }

    public override bool HasPurchasesAvailable()
    {
        if(unlockedLevel < maxLevel)
        {
            return true;
        }

        return false;
    }
    
    public override int GetNextPurchasePrice()
    {
        return CurrentMachineData.upgradePrize;
    }

    public void UpgradeMachine()
    {
        if(unlockedLevel == maxLevel)
        {
            Debug.LogError("[UPGRADES] Failed machine upgrade, the machine is at maximum level");
            return;
        }
        
        unlockedLevel++;
        PlayerPrefs.SetInt(LEVEL_KEY, unlockedLevel);

        LevelChangedAction?.Invoke(unlockedLevel);

        if(unlockedLevel == maxLevel)
        {
            UpgradesMaxedAction?.Invoke();
        }
    }

    public GameObject GetDefaultMachine()
    {
        return defaultMachine;
    }
    
    public GameObject GetMachine()
    {
        return machineDatas[unlockedLevel - 1].Machine3DModel;
    }

    public Color GetInkColor()
    {
        return machineDatas[unlockedLevel - 1].InkColor;
    }
}

[System.Serializable]
public class MachineData
{
    public GameObject Machine3DModel;
    public Color InkColor;
    public int upgradePrize;
}