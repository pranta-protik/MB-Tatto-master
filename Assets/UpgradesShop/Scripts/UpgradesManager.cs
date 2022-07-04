using HomaGames.HomaConsole.Performance.Utils;
using UnityEngine;

public class UpgradesManager : Performance_Singleton<UpgradesManager>
{
    [SerializeField] private UpgradesModel upgradesModel;

    public MachineUpgradeSO MachineUpgradeSo => upgradesModel.machineUpgrade;
    public TattooUpgradeSO SelectedTattooUpgrade => upgradesModel.selectedTattooUpgrade; 
    public JewelryUpgradeSO SelectedJewelryUpgrade => upgradesModel.selectedJewelryUpgrade;
}