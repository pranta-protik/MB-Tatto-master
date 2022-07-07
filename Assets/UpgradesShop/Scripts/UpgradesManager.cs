using HomaGames.HomaConsole.Performance.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradesManager : Performance_Singleton<UpgradesManager>
{
    [SerializeField] private UpgradesModel upgradesModel;
    [SerializeField] private ExitShopTrigger exitShopTrigger;
    [SerializeField] private string sceneName;

    public MachineUpgradeSO MachineUpgradeSo => upgradesModel.machineUpgrade;
    public TattooUpgradeSO SelectedTattooUpgrade => upgradesModel.selectedTattooUpgrade; 
    public JewelryUpgradeSO SelectedJewelryUpgrade => upgradesModel.selectedJewelryUpgrade;

    protected override void Awake()
    {
        base.Awake();
        
        exitShopTrigger.ExitShopAction += OnExitShop;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        exitShopTrigger.ExitShopAction += OnExitShop;
    }

    private void OnExitShop()
    {
        SceneManager.LoadScene(sceneName);
    }
}