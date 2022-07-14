using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MachineStation : UpgradeStation
{
    #region Params
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private ParticleSystem tattooGunUpgradeEffect;
    
    private MachineUpgradeSO machineUpgradeData;
    private GameObject currentLevelObj;
    private Transform currentLevelPreview;
    #endregion

    #region Init&Mono
    protected override void Awake()
    {
        base.Awake();

        machineUpgradeData = upgradeData as MachineUpgradeSO;
        machineUpgradeData.LevelChangedAction += OnLevelChanged;
    }

    protected override void Start()
    {
        base.Start();
        
        ActivateMachineLevel();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        machineUpgradeData.LevelChangedAction -= OnLevelChanged;
    }
    #endregion

    #region Handlers
    private void OnLevelChanged(int newLevel)
    {
        currentLevelObj.SetActive(false);
        currentLevelObj = levels[newLevel - 1];
        currentLevelPreview = currentLevelObj.transform.GetChild(0);
        currentLevelObj.SetActive(true);
        tattooGunUpgradeEffect.Play();
    }
    #endregion

    #region Logic
    public override void UpscaleBigPreview()
    {
        originalPreviewScale = currentLevelPreview.localScale;

        if(upscaleTween != null)
        {
            upscaleTween.Kill();
            upscaleTween = null;
        }
        
        upscaleTween = currentLevelPreview.DOScale(originalPreviewScale * upscaleValue, scaleDuration).OnComplete(() =>
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
        
        upscaleTween = currentLevelPreview.DOScale(originalPreviewScale, scaleDuration).OnComplete(() =>
        {
            upscaleTween = null;
        });
    }
    
    private void ActivateMachineLevel()
    {
        for(int i = 0, count = levels.Count; i < count; i++)
        {
            if(i == machineUpgradeData.UnlockedLevel - 1)
            {
                levels[i].SetActive(true);
                currentLevelObj = levels[i];
                currentLevelPreview = currentLevelObj.transform.GetChild(0);
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
    }
    #endregion
}