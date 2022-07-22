using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text dollarText;
    [SerializeField] private float popUpFinalYPosition;
    [SerializeField] private float popUpMoveDuration;
    [SerializeField] private AnimatorOverrideController animatorOverrideController;
    [SerializeField] private Image emojiPopUp;
    [SerializeField] private List<Sprite> emojis;

    private bool _isPopUpEnabled;
    private bool _isPopUpActive;
    private float _timer;
    private float _startTimer;

    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    public void SetupEmojis()
    {
        _startTimer = Random.Range(3f, 5f);
        _timer = _startTimer;
        
        int index = Random.Range(0, emojis.Count);
        emojiPopUp.sprite = emojis[index];
        
        _isPopUpActive = true;
        _isPopUpEnabled = true;
    }
    
    private void Update()
    {
        if (!_isPopUpActive) return;
        
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _isPopUpEnabled = !_isPopUpEnabled;
            _timer = _startTimer;
            
            int index = Random.Range(0, emojis.Count);
            emojiPopUp.sprite = emojis[index];

            if (_isPopUpEnabled)
            {
                emojiPopUp.gameObject.SetActive(_isPopUpEnabled);

                emojiPopUp.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f).OnComplete(() =>
                {
                    emojiPopUp.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.7f).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0f, 1f));
                });    
            }
            else
            {
                emojiPopUp.transform.DOKill();
                emojiPopUp.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                {
                    emojiPopUp.gameObject.SetActive(false); 
                });
            }
            
        }
    }

    public void HideEmojis()
    {
        _isPopUpActive = false;
        emojiPopUp.transform.DOKill();
        emojiPopUp.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            emojiPopUp.gameObject.SetActive(false); 
        });
    }

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