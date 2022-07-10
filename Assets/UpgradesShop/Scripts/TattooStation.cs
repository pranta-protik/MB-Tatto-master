using DG.Tweening;
using UnityEngine;

public class TattooStation : UpgradeStation
{
    [SerializeField] private Transform bigPreviewContainer;
    [SerializeField] private SpriteRenderer smallPreviewSpriteRenderer;
    [SerializeField] private SpriteRenderer bigPreviewSpriteRenderer;

    private TattooUpgradeSO tattooUpgradeData;

    protected override void Awake()
    {
        base.Awake();

        tattooUpgradeData = upgradeData as TattooUpgradeSO;
        originalPreviewScale = bigPreviewContainer.transform.localScale;

        // Setup 2D sprites if station is available
        if (upgradeData.IsAvailable)
        {
            SetPreviewSprites();   
        }
    }

    public override void UpscaleBigPreview()
    {
        if(upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }

        upscaleTween = bigPreviewContainer.transform.DOScale(originalPreviewScale * upscaleValue, scaleDuration).OnComplete(() => { upscaleTween = null; });
    }

    public override void DownscaleBigPreview()
    {
        if(upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }

        upscaleTween = bigPreviewContainer.transform.DOScale(originalPreviewScale, scaleDuration).OnComplete(() => { upscaleTween = null; });
    }

    private void SetPreviewSprites()
    {
        smallPreviewSpriteRenderer.sprite = tattooUpgradeData.GetTattoo();
        bigPreviewSpriteRenderer.sprite = tattooUpgradeData.GetTattoo();
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        SetPreviewSprites();
    }
}