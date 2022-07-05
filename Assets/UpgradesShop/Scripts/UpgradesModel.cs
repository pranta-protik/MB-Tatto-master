using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesModel : MonoBehaviour
{
    #region Params
    public MachineUpgradeSO machineUpgrade;
    public List<TattooUpgradeSO> tattooUpgrades; //The order of this list defines the unlock order
    public List<JewelryUpgradeSO> jewelryUpgrades; //The order of this list defines the unlock order

    [HideInInspector] public TattooUpgradeSO selectedTattooUpgrade;
    [HideInInspector] public JewelryUpgradeSO selectedJewelryUpgrade;

    private int tattooSelectedIndex;
    private int jewelrySelectedIndex;

    private const string TATTOO_INDEX_KEY = "TattooIndexKey";
    private const string JEWELRY_INDEX_KEY = "JewelryIndexKey";
    #endregion

    #region Init&Mono
    private void Awake()
    {
        tattooSelectedIndex = PlayerPrefs.GetInt(TATTOO_INDEX_KEY, 0);
        jewelrySelectedIndex = PlayerPrefs.GetInt(JEWELRY_INDEX_KEY, 0);

        selectedTattooUpgrade = tattooUpgrades[tattooSelectedIndex];
        selectedJewelryUpgrade = jewelryUpgrades[jewelrySelectedIndex];

        for(int i = 0, count = tattooUpgrades.Count; i < count; i++)
        {
            tattooUpgrades[i].UpgradeUnlockedAction += OnUpgradeUnlocked;
        }

        for(int i = 0, count = jewelryUpgrades.Count; i < count; i++)
        {
            jewelryUpgrades[i].UpgradeUnlockedAction += OnUpgradeUnlocked;
        }
    }

    private void Start()
    {
        machineUpgrade.Activate();
        selectedTattooUpgrade.Activate();
        selectedJewelryUpgrade.Activate();
    }

    private void OnDestroy()
    {
        for(int i = 0, count = tattooUpgrades.Count; i < count; i++)
        {
            tattooUpgrades[i].UpgradeUnlockedAction -= OnUpgradeUnlocked;
        }

        for(int i = 0, count = jewelryUpgrades.Count; i < count; i++)
        {
            jewelryUpgrades[i].UpgradeUnlockedAction -= OnUpgradeUnlocked;
        }
    }
    #endregion

    #region Handlers
    private void OnUpgradeUnlocked(UpgradeDataSO upgrade)
    {
        switch(upgrade.upgradeType)
        {
            case UpgradeType.TattooDesign:
                if(tattooSelectedIndex < tattooUpgrades.Count - 1)
                {
                    tattooSelectedIndex++;
                    ActivateNextUpgrade(upgrade.upgradeType);
                }

                break;
            case UpgradeType.Jewelry:
                if(jewelrySelectedIndex < jewelryUpgrades.Count - 1)
                {
                    jewelrySelectedIndex++;
                    ActivateNextUpgrade(upgrade.upgradeType);
                }

                break;
        }

        SelectUpgrade(upgrade);
    }
    #endregion

    #region Logic
    public void SelectUpgrade(UpgradeDataSO upgrade)
    {
        switch(upgrade.upgradeType)
        {
            case UpgradeType.TattooDesign:
                selectedTattooUpgrade = upgrade as TattooUpgradeSO;
                break;
            case UpgradeType.Jewelry:
                selectedJewelryUpgrade = upgrade as JewelryUpgradeSO;
                break;
        }
    }

    public void ActivateNextUpgrade(UpgradeType upgradeType)
    {
        switch(upgradeType)
        {
            case UpgradeType.TattooDesign:
                if(tattooSelectedIndex < tattooUpgrades.Count - 1)
                {
                    tattooUpgrades[tattooSelectedIndex + 1].Activate();
                }

                break;
            case UpgradeType.Jewelry:
                if(jewelrySelectedIndex < jewelryUpgrades.Count - 1)
                {
                    jewelryUpgrades[jewelrySelectedIndex + 1].Activate();
                }

                break;
        }
    }
    #endregion
}