using System.Collections.Generic;
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
    [SerializeField] private List<Renderer> renderersToGreyscale;
    [SerializeField] private List<Renderer> renderersToBlack;

    protected Vector3 originalPreviewScale;
    protected Tweener upscaleTween;
    private bool hasUsedGreyscale = false;
    private bool hasUsedNullTexture = false;
    private List<Shader> originalShaders;
    private List<Texture2D> originalTextures;
    private List<Color> originalColors;

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

    protected virtual void Start()
    {
        payPlatform.Init(upgradeData);
        
        // Keep all stations available from start
        // Use black silhouette
        // gameObject.SetActive(upgradeData.IsAvailable);
        
        if (!upgradeData.IsAvailable)
        {
            SetAvailabilityState(upgradeData.IsAvailable);
        }
        else
        {
            SetUnlockState(!upgradeData.IsUnlocked);   
        }
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

        if (upgradeData.IsAvailable)
        {
            UpscaleBigPreview();    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != PlayerTag)
        {
            return;
        }

        if (upgradeData.IsAvailable)
        {
            DownscaleBigPreview();
        }
    }

    protected virtual void OnActivate()
    {
        payPlatform.Init(upgradeData);
        // Remove black silhouette
        // gameObject.SetActive(true);
        SetAvailabilityState(true);
        SetUnlockState(!upgradeData.IsUnlocked);   
    }

    protected virtual void OnUnlocked(UpgradeDataSO upgrade)
    {
        SetUnlockState(false);
    }
    #endregion

    #region Logic
   
    private void SetUnlockState(bool isLocked)
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

    private void SetAvailabilityState(bool isAvailable)
    {
        if (!isAvailable)
        {
            originalTextures = new List<Texture2D>();
            originalColors = new List<Color>();

            for (int i = 0, count = renderersToBlack.Count; i < count; i++)
            {
                originalTextures.Add(renderersToBlack[i].material.mainTexture as Texture2D);
                originalColors.Add(renderersToBlack[i].material.color);
                
                renderersToBlack[i].material.mainTexture = null;
                renderersToBlack[i].material.color = Color.black;
            }

            hasUsedNullTexture = true;
        }
        else if (hasUsedNullTexture)
        {
            for (int i = 0, count = renderersToBlack.Count; i < count; i++)
            {
                renderersToBlack[i].material.mainTexture = originalTextures[i];
                renderersToBlack[i].material.color = originalColors[i];
            }

            hasUsedNullTexture = false;
        }
    }
    
    #endregion
}