using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Upgrades/MachineUpgradeData")]
public class MachineUpgradeSO : UpgradeDataSO
{
    [FormerlySerializedAs("machineData")] [FormerlySerializedAs("machineAssets")] public List<MachineData> machineDatas;
    
    private int unlockedLevel;
    
    public int maxLevel => machineDatas.Count;
    public int UnlockedLevel => unlockedLevel;
    public MachineData CurrentMachineData => machineDatas[unlockedLevel];

    public Action<int> LevelChangedAction;
    
    private const string LEVEL_KEY = "LevelKey";

    private void Awake()
    {
        upgradeType = UpgradeType.TattooMachine;
        unlockedLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public override void Deposit(int amount)
    {
        CurrencyDeposited += amount;

        if(CurrencyDeposited >= GetPriceSum())
        {
            UpgradeMachine();

            if(CurrencyDeposited > GetPriceSum() && unlockedLevel == maxLevel)
            {
                Debug.LogError("[UPGRADES] To much money deposited for machine upgrade");
            }
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

    private int GetPriceSum()
    {
        int finalPrice = 0;
        for(int i = 0; i < unlockedLevel; i++)
        {
            finalPrice += machineDatas[i].upgradePrize;
        }

        return finalPrice;
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