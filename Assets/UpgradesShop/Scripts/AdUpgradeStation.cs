using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AdUpgradeStation : MonoBehaviour
{
    [SerializeField] private string upgradeName;
    [SerializeField] private GameObject lockedContainer;
    [SerializeField] private WatchAdPlatform watchAdPlatform;
    [SerializeField] private Shader greyscaleShader;
    [SerializeField] protected float upscaleValue = 1.5f;
    [SerializeField] protected float scaleDuration = 0.33f;
    [SerializeField] private List<Renderer> renderersToGreyscale;
    [SerializeField] protected Transform bigPreviewContainer;
    
    private bool isUnlocked;
    private string unlockedKey;
    
    public bool IsUnlocked => isUnlocked;
    
    private bool hasUsedGreyscale = false;
    private List<Shader> originalShaders;
    private Vector3 originalPreviewScale;
    private Tweener upscaleTween;
    
    private const string PlayerTag = "Player";
    
    protected virtual void Start()
    {
        unlockedKey = string.Concat("UnlockedKey", "_", upgradeName);
        isUnlocked = PlayerPrefs.GetInt(unlockedKey, 0) == 1;

        watchAdPlatform.Init(!isUnlocked);
        SetUnlockStatus(!isUnlocked);
        
        originalPreviewScale = bigPreviewContainer.transform.localScale;
    }
    
    private void SetUnlockStatus(bool isLocked)
    {
        if(lockedContainer != null)
        {
            lockedContainer.SetActive(isLocked);
        }

        if(isLocked)
        {
            originalShaders = new List<Shader>();
            
            for(int i = 0, count = renderersToGreyscale.Count; i < count; i++)
            {
                originalShaders.Add(renderersToGreyscale[i].material.shader);
                renderersToGreyscale[i].material.shader = greyscaleShader;
            }

            hasUsedGreyscale = true;
        }
        else if(hasUsedGreyscale)
        {
            for(int i = 0, count = renderersToGreyscale.Count; i < count; i++)
            {
                renderersToGreyscale[i].material.shader = originalShaders[i];
            }
            
            hasUsedGreyscale = false;
        }
    }
    
    public void UnlockStation()
    {
        PlayerPrefs.SetInt(unlockedKey, 1);
        SetUnlockStatus(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(PlayerTag))
        {
            return;
        }
        
        UpscaleBigPreview();   
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(PlayerTag))
        {
            return;
        }

        DownscaleBigPreview();
    }
    
    private void UpscaleBigPreview()
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

    private void DownscaleBigPreview()
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
}
