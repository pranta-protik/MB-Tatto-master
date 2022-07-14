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
            bigPreviewContainer.gameObject.SetActive(true);
        }
        else
        {
            bigPreviewContainer.gameObject.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_AMOUNT, 0) == 1)
        {
            int serial = PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_TATTOO_INDEX, 0);

            if (serial == tattooUpgradeData.serialNo)
            {
                equipmentPlatform.gameObject.SetActive(false);
                unequipmentPlatform.gameObject.SetActive(true);
            }
        }
    }

    public bool IsStationUnlocked()
    {
        return upgradeData.IsUnlocked;
    }

    public override void UpscaleBigPreview()
    {
        if (upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }

        upscaleTween = bigPreviewContainer.transform.DOScale(originalPreviewScale * upscaleValue, scaleDuration).OnComplete(() => { upscaleTween = null; });
    }

    public override void DownscaleBigPreview()
    {
        if (upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }

        upscaleTween = bigPreviewContainer.transform.DOScale(originalPreviewScale, scaleDuration).OnComplete(() => { upscaleTween = null; });
    }

    private void SetPreviewSprites()
    {
        smallPreviewSpriteRenderer.sprite = tattooUpgradeData.GetPreviewTattoo();
        bigPreviewSpriteRenderer.sprite = tattooUpgradeData.GetPreviewTattoo();
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        SetPreviewSprites();
        bigPreviewContainer.gameObject.SetActive(true);
    }


    public override int GetSerialNo()
    {
        return tattooUpgradeData.serialNo;
    }

    protected override void OnUnlocked(UpgradeDataSO upgrade)
    {
        base.OnUnlocked(upgrade);

        UpgradesManager.Instance.ClearAllTattooStations();
        
        if (equipmentPlatform != null && unequipmentPlatform != null)
        {
            equipmentPlatform.gameObject.SetActive(false);
            unequipmentPlatform.gameObject.SetActive(true);
            
            PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + unequipmentPlatform.GetUpgradeType() + unequipmentPlatform.GetSerialNo(), 1);
        }
    }
}