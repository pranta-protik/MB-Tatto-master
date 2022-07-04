using UnityEngine;

public class TattooStation : UpgradeStation
{
    [SerializeField] private SpriteRenderer smallPreviewSpriteRenderer;
    [SerializeField] private SpriteRenderer bigPreviewSpriteRenderer;

    private TattooUpgradeSO tattoUpgradeData;

    protected override void Awake()
    {
        base.Awake();
        
        tattoUpgradeData = upgradeData as TattooUpgradeSO;

        SetPreviewSprites();
    }

    private void SetPreviewSprites()
    {
        smallPreviewSpriteRenderer.sprite = tattoUpgradeData.GetTattoo();
        bigPreviewSpriteRenderer.sprite = tattoUpgradeData.GetTattoo();
    }
}