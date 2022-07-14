using HomaGames.HomaConsole.Performance.Utils;
using UnityEngine;

public class UpgradesManager : Performance_Singleton<UpgradesManager>
{
    [SerializeField] private UpgradesModel upgradesModel;
    public float interstitialAdTimer = 45f;

    public MachineUpgradeSO MachineUpgradeSo => upgradesModel.machineUpgrade;
    public TattooUpgradeSO SelectedTattooUpgrade => upgradesModel.selectedTattooUpgrade;
    public JewelryUpgradeSO SelectedJewelryUpgrade => upgradesModel.selectedJewelryUpgrade;

    protected override void Awake()
    {
        base.Awake();
        
        DontDestroyOnLoad(this);
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    public void AddCash()
    {
        StorageManager.SetTotalScore(100000);
    }

    [Sirenix.OdinInspector.Button]
    public void DisplayJewelName()
    {
        GameObject jewel = GetJewel();
        Debug.Log(jewel.name);
    }

    [Sirenix.OdinInspector.Button]
    public void DisplayTattooName()
    {
        Sprite tattoo = GetTattoo();
        Debug.Log(tattoo.name);
    }
#endif

    public void ClearAllJewelryStations()
    {
        upgradesModel.ClearAllJewelryStations();
    }

    public void ClearAllTattooStations()
    {
        upgradesModel.ClearAllTattooStations();
    }

    public void ClearJewelryStation(int serialNo)
    {
        upgradesModel.ClearJewelryStation(serialNo - 1);
    }

    public void ClearTattooStation(int serialNo)
    {
        upgradesModel.ClearTattooStation(serialNo - 1);
    }

    public GameObject GetTattooGun()
    {
        if(!MachineUpgradeSo.IsAvailable || !MachineUpgradeSo.IsUnlocked)
        {
            return MachineUpgradeSo.GetDefaultMachine();
        }
        
        return MachineUpgradeSo.GetMachine();
    }

    public Color GetInkColor()
    {
        if(!MachineUpgradeSo.IsAvailable || !MachineUpgradeSo.IsUnlocked)
        {
            return Color.black;
        }
        
        return MachineUpgradeSo.GetInkColor();
    }

    public Sprite GetTattoo()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 0)==1)
        {
            int index = PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, 0) - 1;

            if (index < upgradesModel.tattooUpgrades.Count)
            {
                return upgradesModel.tattooUpgrades[index].GetTattoo();
            }

            if (index < upgradesModel.tattooUpgrades.Count + upgradesModel.tattooUpgradeSprites.Count)
            {
                return upgradesModel.tattooUpgradeSprites[index - upgradesModel.tattooUpgrades.Count];
            }

            return null;
        }

        return null;
    }

    public GameObject GetJewel()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_AMOUNT, 0) == 1)
        {
            int index = PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_INDEX, 0) - 1;

            if (index < upgradesModel.jewelryUpgrades.Count)
            {
                return upgradesModel.jewelryUpgrades[index].GetJewelry();    
            }
            
            if (index < upgradesModel.jewelryUpgrades.Count + upgradesModel.jewelryUpgradePrefabs.Count)
            {
                return upgradesModel.jewelryUpgradePrefabs[index - upgradesModel.jewelryUpgrades.Count];    
            }

            return null;
        }
        
        return null;
    }
}