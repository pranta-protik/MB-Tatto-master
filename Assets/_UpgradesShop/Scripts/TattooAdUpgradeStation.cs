using UnityEngine;

public class TattooAdUpgradeStation : AdUpgradeStation
{
    [SerializeField] private Sprite previewTattooSprite;
    [SerializeField] private SpriteRenderer smallPreviewSpriteRenderer;
    [SerializeField] private SpriteRenderer bigPreviewSpriteRenderer;
    
    protected override void Start()
    {
        base.Start();
        
        if (isUnlocked)
        {
            equipmentPlatform.gameObject.SetActive(true);
            unequipmentPlatform.gameObject.SetActive(false);
            
            if (PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 0) == 1)
            {
                int serial = PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, 0);

                if (serial == serialNo)
                {
                    equipmentPlatform.gameObject.SetActive(false);
                    unequipmentPlatform.gameObject.SetActive(true);
                }
            }
        }
        
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
        smallPreviewSpriteRenderer.sprite = previewTattooSprite;
        bigPreviewSpriteRenderer.sprite = previewTattooSprite;
    }
}