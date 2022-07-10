using HomaGames.HomaConsole.Performance.Utils;
using UnityEngine;

public class UpgradesManager : Performance_Singleton<UpgradesManager>
{
    [SerializeField] private UpgradesModel upgradesModel;
    // [SerializeField] private ExitShopTrigger exitShopTrigger;
    [SerializeField] private string sceneName;

    public MachineUpgradeSO MachineUpgradeSo => upgradesModel.machineUpgrade;
    public TattooUpgradeSO SelectedTattooUpgrade => upgradesModel.selectedTattooUpgrade;
    public JewelryUpgradeSO SelectedJewelryUpgrade => upgradesModel.selectedJewelryUpgrade;

    protected override void Awake()
    {
        base.Awake();
        
        DontDestroyOnLoad(this);
        
        // exitShopTrigger.ExitShopAction += OnExitShop;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // exitShopTrigger.ExitShopAction += OnExitShop;
    }

    // private void OnExitShop()
    // {
    //     SceneManager.LoadScene(sceneName);
    // }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    public void AddCash()
    {
        StorageManager.SetTotalScore(100000);
    }
#endif

    public string GetSceneName()
    {
        return sceneName;
    }
    
    public GameObject GetTatgun()
    {
        return MachineUpgradeSo.GetMachine();
    }

    public Color GetInkColor()
    {
        return MachineUpgradeSo.GetInkColor();
    }

    public Sprite GetTattoo()
    {
        return SelectedTattooUpgrade.GetTattoo();
    }

    public GameObject GetJewel()
    {
        return SelectedJewelryUpgrade.GetJewelry();
    }
}