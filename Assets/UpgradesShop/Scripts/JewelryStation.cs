using DG.Tweening;
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
        originalPreviewScale = bigPreviewContainer.transform.localScale;
        
        Set3DModelPreview();
    }

    public override void UpscaleBigPreview()
    {

        if(upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }
        
        upscaleTween = bigPreviewContainer.transform.DOScale(originalPreviewScale * upscaleValue, scaleDuration).OnComplete(() =>
        {
            upscaleTween = null;
        });
    }

    public override void DownscaleBigPreview()
    {
        if(upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }
        
        upscaleTween = bigPreviewContainer.transform.DOScale(originalPreviewScale, scaleDuration).OnComplete(() =>
        {
            upscaleTween = null;
        });
    }

    public void Set3DModelPreview()
    {
        Instantiate(jewelryUpgradeData.GetJewelry(), smallPreviewContainer.transform);
        Instantiate(jewelryUpgradeData.GetJewelry(), bigPreviewContainer.transform);
    }
}