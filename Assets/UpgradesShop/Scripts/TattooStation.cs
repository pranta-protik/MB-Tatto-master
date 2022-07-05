using DG.Tweening;
using UnityEngine;

public class TattooStation : UpgradeStation
{
    [SerializeField] private Transform bigPreviewContainer;
    [SerializeField] private SpriteRenderer smallPreviewSpriteRenderer;
    [SerializeField] private SpriteRenderer bigPreviewSpriteRenderer;

    private TattooUpgradeSO tattoUpgradeData;

    protected override void Awake()
    {
        base.Awake();
        
        tattoUpgradeData = upgradeData as TattooUpgradeSO;

        SetPreviewSprites();
    }
    
    public override void UpscaleBigPreview()
    {
        originalPreviewScale = bigPreviewContainer.transform.localScale;

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

    private void SetPreviewSprites()
    {
        smallPreviewSpriteRenderer.sprite = tattoUpgradeData.GetTattoo();
        bigPreviewSpriteRenderer.sprite = tattoUpgradeData.GetTattoo();
    }
}