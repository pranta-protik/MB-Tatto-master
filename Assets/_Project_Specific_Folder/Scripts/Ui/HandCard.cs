using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandCard : MonoBehaviour
{
    public enum ECardType
    {
        Model,
        Image
    }
    public enum ERequirementType
    {
        Cash,
        Time,
        GamePlay,
        Level
    }

    [Header("Requirement Section")]
    public ECardType cardType;
    public ERequirementType requirementType;
    public int handId;
    public int requiredCash;
    public float requiredTime;
    public int requiredMatches;
    public int requiredLevelNo;
    public int cardStatus;
    
    [Header("Animation Section")]
    public AnimatorOverrideController animatorOverrideController;
    public AnimationClip[] animationClips;
    private bool _isAnimationPlaying;
    private Animator _animator;
    private static readonly int IsSelected = Animator.StringToHash("isSelected");

    private void Start()
    {
        if (cardType == ECardType.Model)
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController
            {
                runtimeAnimatorController = _animator.runtimeAnimatorController
            };
            int animationClipIndex = Random.Range(0, animationClips.Length);
            animatorOverrideController["gesture02"] = animationClips[animationClipIndex];

            _animator.runtimeAnimatorController = animatorOverrideController;   
        }
    }

    public void UpdateCardStatus()
    {
        cardStatus = 1;
        PlayerPrefs.SetInt("HandCard" + handId, cardStatus);
    }

    public void PlayRandomAnimation()
    {
        if (!_isAnimationPlaying)
        {
            int animationClipIndex = Random.Range(0, animationClips.Length);
            animatorOverrideController["gesture01"] = animationClips[animationClipIndex];

            _animator.runtimeAnimatorController = animatorOverrideController;
            _animator.SetTrigger(IsSelected);
            
            _isAnimationPlaying = true;
        }
    }

    public void PlayIdleAnimation()
    {
        _isAnimationPlaying = false;
    }
}
