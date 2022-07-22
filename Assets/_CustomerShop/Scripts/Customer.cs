using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text dollarText;
    [SerializeField] private float popUpFinalYPosition;
    [SerializeField] private float popUpMoveDuration;
    [SerializeField] private AnimatorOverrideController animatorOverrideController;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    public void SetIdleAnimation(AnimationClip animationClip)
    {
        animatorOverrideController = new AnimatorOverrideController
        {
            runtimeAnimatorController = _animator.runtimeAnimatorController,
            ["Breathing Idle"] = animationClip
        };

        _animator.runtimeAnimatorController = animatorOverrideController;
    }

    public void Move(Transform[] exitPositions)
    {
        dollarText.enabled = true;
        dollarText.transform.DOLocalMoveY(popUpFinalYPosition, popUpMoveDuration);
        dollarText.DOFade(0, popUpMoveDuration);
        
        transform.LookAt(exitPositions[0]);
        transform.GetChild(0).LookAt(exitPositions[0]);
        
        _animator.SetBool(IsWalking, true);
        transform.DOMove(exitPositions[0].position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.LookAt(exitPositions[1]);
            transform.GetChild(0).LookAt(exitPositions[1]);
            transform.DOMove(exitPositions[1].position, 3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}