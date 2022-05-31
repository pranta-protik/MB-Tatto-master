using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GateMover : MonoBehaviour
{
    public enum  EMoveDirection
    {
        Left, Right
    }

    public EMoveDirection moveDirection;
    public float moveAmount = 0.5f;
    public float duration = 0.5f;

    private void Start()
    {
        if (moveDirection == EMoveDirection.Right)
        {
            moveAmount *= -1f;
        }
        Vector3 finalPosition = transform.position + new Vector3(0f, 0f, moveAmount);
        transform.DOMove(finalPosition, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
