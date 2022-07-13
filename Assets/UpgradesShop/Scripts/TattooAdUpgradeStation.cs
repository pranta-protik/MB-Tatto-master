using UnityEngine;

public class TattooAdUpgradeStation : AdUpgradeStation
{
    [SerializeField] private Sprite tattooSprite;
    [SerializeField] private SpriteRenderer smallPreviewSpriteRenderer;
    [SerializeField] private SpriteRenderer bigPreviewSpriteRenderer;
    
    protected override void Start()
    {
        base.Start();
        
        SetPreviewSprites();
    }

    private void SetPreviewSprites()
    {
        smallPreviewSpriteRenderer.sprite = tattooSprite;
        bigPreviewSpriteRenderer.sprite = tattooSprite;
    }
}