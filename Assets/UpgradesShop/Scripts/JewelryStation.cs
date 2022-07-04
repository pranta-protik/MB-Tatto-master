using UnityEngine;

public class JewelryStation : UpgradeStation
{
    [SerializeField] private Transform smallPreviewContainer;
    [SerializeField] private Transform bigPreviewContainer;

    private JewelryUpgradeSO jewelryUpgradeData;

    protected override void Awake()
    {
        base.Awake();
        
        jewelryUpgradeData = upgradeData as JewelryUpgradeSO;

        Set3DModelPreview();
    }
    
    public void Set3DModelPreview()
    {
        Instantiate(jewelryUpgradeData.GetJewelry(), smallPreviewContainer.transform);
        Instantiate(jewelryUpgradeData.GetJewelry(), bigPreviewContainer.transform);
    }
}