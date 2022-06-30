using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/MachineUpgradeData")]
public class MachineUpgradeSO : UpgradeDataSO
{
    public List<MachineAssets> machineAssets;
    
    [SerializeField] private List<int> upgradePrices;

    private int unlockedLevel;
    
    public int maxLevel => upgradePrices.Count;
    public int UnlockedLevel => unlockedLevel;
    
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
    
    public void UpgradeMachine()
    {
        if(unlockedLevel == maxLevel)
        {
            Debug.LogError("[UPGRADES] Failed machine upgrade, the machine is at maximum level");
            return;
        }
        
        unlockedLevel++;
        PlayerPrefs.SetInt(LEVEL_KEY, unlockedLevel);
    }

    private int GetPriceSum()
    {
        int finalPrice = 0;
        for(int i = 0; i < unlockedLevel; i++)
        {
            finalPrice += upgradePrices[i];
        }

        return finalPrice;
    }
    
    public GameObject GetMachine()
    {
        return machineAssets[unlockedLevel - 1].Machine3DModel;
    }

    public Color GetInkColor()
    {
        return machineAssets[unlockedLevel - 1].InkColor;
    }
}

[System.Serializable]
public class MachineAssets
{
    public GameObject Machine3DModel;
    public Color InkColor;
}