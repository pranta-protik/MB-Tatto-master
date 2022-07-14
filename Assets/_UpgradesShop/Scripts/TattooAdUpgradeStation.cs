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

    public override void UnlockStation()
    {
        base.UnlockStation();
        PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, serialNo);
        PlayerPrefs.SetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 1);
        
        UpgradesManager.Instance.ClearAllTattooStations();
        
        equipmentPlatform.gameObject.SetActive(false);
        unequipmentPlatform.gameObject.SetActive(true);
        
        PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + unequipmentPlatform.GetUpgradeType() + unequipmentPlatform.GetSerialNo(), 1);
    }

    public bool IsStationUnlocked()
    {
        return isUnlocked;
    }

    private void SetPreviewSprites()
    {
        smallPreviewSpriteRenderer.sprite = tattooSprite;
        bigPreviewSpriteRenderer.sprite = tattooSprite;
    }
}