using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandCard : MonoBehaviour
{
    public enum ERequirementType
    {
        Cash,
        Time,
        GamePlay,
        Level
    }
    public int handId;
    public ERequirementType requirementType;
    public int requiredCash;
    public float requiredTime;
    public int requiredMatches;
    public int requiredLevelNo;
    public int unlockStatus;
    
    public AnimatorOverrideController animatorOverrideController;
    public AnimationClip[] animationClips;
    private bool _isAnimationPlaying;
    private Animator _animator;
    private static readonly int IsSelected = Animator.StringToHash("isSelected");

    private void Start()
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

    public void PlayRandomAnimation()
    {
        if (!_isAnimationPlaying)
        {
            int animationClipIndex = Random.Range(0, animationClips.Length);
            animatorOverrideController["gesture02"] = animationClips[animationClipIndex];

            _animator.runtimeAnimatorController = animatorOverrideController;
            _animator.SetBool(IsSelected, true);
            
            _isAnimationPlaying = true;
        }
    }

    public void PlayIdleAnimation()
    {
        _animator.SetBool(IsSelected, false);
        _isAnimationPlaying = false;
    }
}
