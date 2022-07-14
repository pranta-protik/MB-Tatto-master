using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradesModel : MonoBehaviour
{
    #region Params

    public MachineUpgradeSO machineUpgrade;
    public List<TattooUpgradeSO> tattooUpgrades; //The order of this list defines the unlock order
    public List<JewelryUpgradeSO> jewelryUpgrades; //The order of this list defines the unlock order
    public List<Sprite> tattooUpgradeSprites;
    public List<GameObject> jewelryUpgradePrefabs;

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

        machineUpgrade.Init();
        for (int i = 0, count = tattooUpgrades.Count; i < count; i++)
        {
            tattooUpgrades[i].Init();
            tattooUpgrades[i].UpgradeUnlockedAction += OnUpgradeUnlocked;
        }

        for (int i = 0, count = jewelryUpgrades.Count; i < count; i++)
        {
            jewelryUpgrades[i].Init();
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
        for (int i = 0, count = tattooUpgrades.Count; i < count; i++)
        {
            tattooUpgrades[i].UpgradeUnlockedAction -= OnUpgradeUnlocked;
        }

        for (int i = 0, count = jewelryUpgrades.Count; i < count; i++)
        {
            jewelryUpgrades[i].UpgradeUnlockedAction -= OnUpgradeUnlocked;
        }
    }

    #endregion

    #region Handlers

    private void OnUpgradeUnlocked(UpgradeDataSO upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.TattooDesign:
                if (tattooSelectedIndex < tattooUpgrades.Count - 1)
                {
                    tattooSelectedIndex++;
                    PlayerPrefs.SetInt(TATTOO_INDEX_KEY, tattooSelectedIndex);

                    PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, tattooUpgrades[tattooSelectedIndex - 1].serialNo);
                    PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 1);

                    ActivateNextUpgrade(upgrade.upgradeType);
                }

                break;

            case UpgradeType.Jewelry:
                if (jewelrySelectedIndex < jewelryUpgrades.Count)
                {
                    jewelrySelectedIndex++;
                    PlayerPrefs.SetInt(JEWELRY_INDEX_KEY, jewelrySelectedIndex);

                    PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_INDEX, jewelryUpgrades[jewelrySelectedIndex - 1].serialNo);
                    PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_AMOUNT, 1);

                    ActivateNextUpgrade(upgrade.upgradeType);
                }

                break;
        }

        SelectUpgrade(upgrade);
    }

    #endregion

    #region Logic

    public void ClearAllJewelryStations()
    {
        for (int i = 0; i < jewelryUpgrades.Count; i++)
        {
            JewelryStation jewelryStation = UpgradeStationsManager.Instance.jewelryUpgradeStations[i].GetComponent<JewelryStation>();

            if (jewelryStation.IsStationUnlocked())
            {
                jewelryStation.equipmentPlatform.gameObject.SetActive(true);
                jewelryStation.unequipmentPlatform.gameObject.SetActive(false);

                PlayerPrefs.SetInt(
                    PlayerPrefsKey.FIRST_TIME_EQUIP + jewelryStation.unequipmentPlatform.GetUpgradeType() + jewelryStation.unequipmentPlatform.GetSerialNo(),
                    0);
            }
        }

        for (int i = jewelryUpgrades.Count; i < UpgradeStationsManager.Instance.jewelryUpgradeStations.Count; i++)
        {
            JewelryAdUpgradeStation jewelryAdUpgradeStation = UpgradeStationsManager.Instance.jewelryUpgradeStations[i].GetComponent<JewelryAdUpgradeStation>();

            if (jewelryAdUpgradeStation.IsStationUnlocked())
            {
                jewelryAdUpgradeStation.equipmentPlatform.gameObject.SetActive(true);
                jewelryAdUpgradeStation.unequipmentPlatform.gameObject.SetActive(false);

                PlayerPrefs.SetInt(
                    PlayerPrefsKey.FIRST_TIME_EQUIP + jewelryAdUpgradeStation.unequipmentPlatform.GetUpgradeType() +
                    jewelryAdUpgradeStation.unequipmentPlatform.GetSerialNo(), 0);
            }
        }
    }

    public void ClearAllTattooStations()
    {
        for (int i = 0; i < tattooUpgrades.Count; i++)
        {
            TattooStation tattooStation = UpgradeStationsManager.Instance.tattooUpgradeStations[i].GetComponent<TattooStation>();

            if (tattooStation.IsStationUnlocked())
            {
                tattooStation.equipmentPlatform.gameObject.SetActive(true);
                tattooStation.unequipmentPlatform.gameObject.SetActive(false);

                PlayerPrefs.SetInt(
                    PlayerPrefsKey.FIRST_TIME_EQUIP + tattooStation.unequipmentPlatform.GetUpgradeType() + tattooStation.unequipmentPlatform.GetSerialNo(), 0);
            }
        }

        for (int i = tattooUpgrades.Count; i < UpgradeStationsManager.Instance.tattooUpgradeStations.Count; i++)
        {
            TattooAdUpgradeStation tattooAdUpgradeStation = UpgradeStationsManager.Instance.tattooUpgradeStations[i].GetComponent<TattooAdUpgradeStation>();

            if (tattooAdUpgradeStation.IsStationUnlocked())
            {
                tattooAdUpgradeStation.equipmentPlatform.gameObject.SetActive(true);
                tattooAdUpgradeStation.unequipmentPlatform.gameObject.SetActive(false);

                PlayerPrefs.SetInt(
                    PlayerPrefsKey.FIRST_TIME_EQUIP + tattooAdUpgradeStation.unequipmentPlatform.GetUpgradeType() +
                    tattooAdUpgradeStation.unequipmentPlatform.GetSerialNo(), 0);
            }
        }
    }

    public void ClearJewelryStation(int serialNo)
    {
        if (serialNo < jewelryUpgrades.Count)
        {
            JewelryStation jewelryStation = UpgradeStationsManager.Instance.jewelryUpgradeStations[serialNo].GetComponent<JewelryStation>();

            if (jewelryStation.IsStationUnlocked())
            {
                jewelryStation.equipmentPlatform.gameObject.SetActive(true);
                jewelryStation.unequipmentPlatform.gameObject.SetActive(false);

                PlayerPrefs.SetInt(
                    PlayerPrefsKey.FIRST_TIME_EQUIP + jewelryStation.unequipmentPlatform.GetUpgradeType() + jewelryStation.unequipmentPlatform.GetSerialNo(),
                    0);
            }
        }
        else
        {
            if (serialNo < UpgradeStationsManager.Instance.jewelryUpgradeStations.Count)
            {
                JewelryAdUpgradeStation jewelryAdUpgradeStation =
                    UpgradeStationsManager.Instance.jewelryUpgradeStations[serialNo].GetComponent<JewelryAdUpgradeStation>();

                if (jewelryAdUpgradeStation.IsStationUnlocked())
                {
                    jewelryAdUpgradeStation.equipmentPlatform.gameObject.SetActive(true);
                    jewelryAdUpgradeStation.unequipmentPlatform.gameObject.SetActive(false);

                    PlayerPrefs.SetInt(
                        PlayerPrefsKey.FIRST_TIME_EQUIP + jewelryAdUpgradeStation.unequipmentPlatform.GetUpgradeType() +
                        jewelryAdUpgradeStation.unequipmentPlatform.GetSerialNo(), 0);
                }
            }
        }
    }

    public void ClearTattooStation(int serialNo)
    {
        if (serialNo < tattooUpgrades.Count)
        {
            TattooStation tattooStation = UpgradeStationsManager.Instance.tattooUpgradeStations[serialNo].GetComponent<TattooStation>();

            if (tattooStation.IsStationUnlocked())
            {
                tattooStation.equipmentPlatform.gameObject.SetActive(true);
                tattooStation.unequipmentPlatform.gameObject.SetActive(false);

                PlayerPrefs.SetInt(
                    PlayerPrefsKey.FIRST_TIME_EQUIP + tattooStation.unequipmentPlatform.GetUpgradeType() + tattooStation.unequipmentPlatform.GetSerialNo(), 0);
            }
        }
        else
        {
            if (serialNo < UpgradeStationsManager.Instance.tattooUpgradeStations.Count)
            {
                TattooAdUpgradeStation tattooAdUpgradeStation =
                    UpgradeStationsManager.Instance.tattooUpgradeStations[serialNo].GetComponent<TattooAdUpgradeStation>();

                if (tattooAdUpgradeStation.IsStationUnlocked())
                {
                    tattooAdUpgradeStation.equipmentPlatform.gameObject.SetActive(true);
                    tattooAdUpgradeStation.unequipmentPlatform.gameObject.SetActive(false);

                    PlayerPrefs.SetInt(
                        PlayerPrefsKey.FIRST_TIME_EQUIP + tattooAdUpgradeStation.unequipmentPlatform.GetUpgradeType() +
                        tattooAdUpgradeStation.unequipmentPlatform.GetSerialNo(), 0);
                }
            }
        }
    }

    public void SelectUpgrade(UpgradeDataSO upgrade)
    {
        switch (upgrade.upgradeType)
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
        switch (upgradeType)
        {
            case UpgradeType.TattooDesign:
                if (tattooSelectedIndex < tattooUpgrades.Count)
                {
                    tattooUpgrades[tattooSelectedIndex].Activate();
                }

                break;
            case UpgradeType.Jewelry:
                if (jewelrySelectedIndex < jewelryUpgrades.Count)
                {
                    jewelryUpgrades[jewelrySelectedIndex].Activate();
                }

                break;
        }
    }

    #endregion
}