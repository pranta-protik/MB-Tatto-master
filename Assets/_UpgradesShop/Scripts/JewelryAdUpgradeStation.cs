using UnityEngine;

public class JewelryAdUpgradeStation : AdUpgradeStation
{
    [SerializeField] private GameObject jewelryPrefab;
    [SerializeField] private Transform smallPreviewContainer;

    protected override void Start()
    {
        base.Start();
        Set3DModelPreview();
    }

    public override void UnlockStation()
    {
        base.UnlockStation();
        
        PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_INDEX, serialNo);
        PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_AMOUNT, 1);
        
        UpgradesManager.Instance.ClearAllJewelryStations();
        
        equipmentPlatform.gameObject.SetActive(false);
        unequipmentPlatform.gameObject.SetActive(true);
        
        PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + unequipmentPlatform.GetUpgradeType() + unequipmentPlatform.GetSerialNo(), 1);
    }

    public bool IsStationUnlocked()
    {
        return isUnlocked;
    }
    
    private void Set3DModelPreview()
    {
        Instantiate(jewelryPrefab, smallPreviewContainer.transform);
        Instantiate(jewelryPrefab, bigPreviewContainer.transform);
    }
}