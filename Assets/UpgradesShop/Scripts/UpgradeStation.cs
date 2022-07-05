using System;
using DG.Tweening;
using UnityEngine;

public abstract class UpgradeStation : MonoBehaviour
{
    #region Params
    [SerializeField] protected UpgradeDataSO upgradeData;
    [SerializeField] private GameObject lockedContainer;
    [SerializeField] private PayPlatform payPlatform;
    [SerializeField] protected float upscaleValue = 1.5f;
    [SerializeField] protected float scaleDuration = 0.33f;

    protected Vector3 originalPreviewScale;
    protected Tweener upscaleTween;
    
    private const string PlayerTag = "Player";
    #endregion

    public abstract void UpscaleBigPreview();
    public abstract void DownscaleBigPreview();

    #region Init&Mono
    protected virtual void Awake()
    {
        upgradeData.UpgradeActivatedAction += OnActivate;
        upgradeData.UpgradeUnlockedAction += OnUnlocked;
    }

    private void Start()
    {
        payPlatform.Init(upgradeData);

        //TEMP disabled
        // gameObject.SetActive(upgradeData.IsAvailable);
        SetState(upgradeData.IsUnlocked);
    }

    protected virtual void OnDestroy()
    {
        upgradeData.UpgradeActivatedAction -= OnActivate;
        upgradeData.UpgradeUnlockedAction -= OnUnlocked;
    }
    #endregion

    #region Handlers
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != PlayerTag)
        {
            return;
        }
        
        UpscaleBigPreview();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != PlayerTag)
        {
            return;
        }
        
        DownscaleBigPreview();
    }

    private void OnActivate()
    {
        gameObject.SetActive(true);
    }

    protected virtual void OnUnlocked(UpgradeDataSO upgrade)
    {
        SetState(false);
    }
    #endregion

    #region Logic
    private void SetState(bool isLocked)
    {
        lockedContainer.SetActive(isLocked);
    }
    #endregion
}