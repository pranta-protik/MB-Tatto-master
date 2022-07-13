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
    
    private void Set3DModelPreview()
    {
        Instantiate(jewelryPrefab, smallPreviewContainer.transform);
        Instantiate(jewelryPrefab, bigPreviewContainer.transform);
    }
}