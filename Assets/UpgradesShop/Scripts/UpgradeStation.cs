using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class UpgradeStation : MonoBehaviour
{
    #region Params
    [SerializeField] protected UpgradeDataSO upgradeData;
    [SerializeField] private GameObject lockedContainer;
    [SerializeField] private PayPlatform payPlatform;
    [SerializeField] private Shader greyscaleShader;
    [SerializeField] protected float upscaleValue = 1.5f;
    [SerializeField] protected float scaleDuration = 0.33f;

    protected Vector3 originalPreviewScale;
    protected Tweener upscaleTween;
    private bool hasUsedGreyscale = false;
    
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
        gameObject.SetActive(upgradeData.IsAvailable);
        SetState(!upgradeData.IsUnlocked);
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
    private List<Renderer> renderers;
    private List<Shader> originalShaders;
    private void SetState(bool isLocked)
    {
        if(lockedContainer != null)
        {
            lockedContainer.SetActive(isLocked);
        }

        if(isLocked)
        {
            renderers = GetComponentsInChildren<Renderer>().ToList();
            originalShaders = new List<Shader>();
            
            for(int i = 0, count = renderers.Count; i < count; i++)
            {
                originalShaders.Add(renderers[i].material.shader);
                renderers[i].material.shader = greyscaleShader;
            }

            hasUsedGreyscale = true;
        }
        else if(hasUsedGreyscale)
        {
            for(int i = 0, count = renderers.Count; i < count; i++)
            {
                renderers[i].material.shader = originalShaders[i];
            }

            hasUsedGreyscale = false;
        }
    }
    #endregion
}