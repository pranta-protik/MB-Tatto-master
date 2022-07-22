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

        // Instantiate 3D models if station is available
        if (upgradeData.IsAvailable)
        {
            Set3DModelPreview();   
        }
    }

    protected override void Start()
    {
        base.Start();
    
        if (PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_AMOUNT, 0) == 1)
        {
            int serial = PlayerPrefs.GetInt(PlayerPrefsKey.EQUIPPED_JEWELRY_INDEX, 0);
    
            if (serial == jewelryUpgradeData.serialNo)
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

    protected override void OnActivate()
    {
        base.OnActivate();
        Set3DModelPreview();
    }

    public override int GetSerialNo()
    {
        return jewelryUpgradeData.serialNo;
    }

    protected override void OnUnlocked(UpgradeDataSO upgrade)
    {
        base.OnUnlocked(upgrade);
        
        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_TWO_STATUS, 0) == 0)
        {
            PointersManager.Instance.DisableSecondTutorialPointer();    
        }
        
        UpgradesManager.Instance.ClearAllJewelryStations();
        
        if (equipmentPlatform != null && unequipmentPlatform!= null)
        {
            equipmentPlatform.gameObject.SetActive(false);
            unequipmentPlatform.gameObject.SetActive(true);
            
            PlayerPrefs.SetInt(PlayerPrefsKey.FIRST_TIME_UNEQUIP + unequipmentPlatform.GetUpgradeType() + unequipmentPlatform.GetSerialNo(), 1);
        }
    }
}