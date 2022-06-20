using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wheel : MonoBehaviour
{
    private int _randomValue;
    private float _timeInterval;
    public bool startSpinning;
    private int _finalAngle;
    private int _multiplier;

    private void Update()
    {
        if (startSpinning)
        {
            StartCoroutine(Spin());
            startSpinning = false;
        }
    }

    private IEnumerator Spin()
    {
        _randomValue = Random.Range(30, 40);
        Debug.Log(_randomValue);
        _timeInterval = 0.05f;

        int i = 0;
        while (i < _randomValue)
        {
            transform.DORotate(new Vector3(0f, 0f, -22.5f), _timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(() =>
            {
                _timeInterval += Time.unscaledDeltaTime * ((float) i / 100);
                i++;

            });

            yield return new WaitForSeconds(_timeInterval);
        }

        if (Mathf.RoundToInt(transform.eulerAngles.z) % 45 != 0)
        {
            transform.DORotate(new Vector3(0f, 0f, -22.5f), _timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(GetResultValue);
        }
        else
        {
            GetResultValue();
        }
    }

    private void GetResultValue()
    {
        _finalAngle = Mathf.RoundToInt(Mathf.Abs(transform.eulerAngles.z));

        switch (_finalAngle)
        {
            case 0:
                _multiplier = 5;
                break;
            case 45:
                _multiplier = 2;
                break;
            case 90:
                _multiplier = 5;
                break;
            case 135:
                _multiplier = 3;
                break;
            case 180:
                _multiplier = 2;
                break;
            case 225:
                _multiplier = 4;
                break;
            case 270:
                _multiplier = 2;
                break;
            case 315:
                _multiplier = 3;
                break;
        }
        
        Invoke(nameof(DisableWheel), 0.5f);
    }

    private void DisableWheel()
    {
        // UiManager.Instance.spinnerScreen.SetActive(false);
        // UiManager.Instance.cashPile.SetActive(true);

        Vector3[] splitPositions = {new Vector3(0f, 0f, 0f), new Vector3(170f, 170f, 0f), new Vector3(-170f, 170f, 0f), new Vector3(170f, -170f, 0f), new Vector3(-170f, -170f, 0f)};
        
        float delay = 0f;
        
        // for (int i = 0; i < UiManager.Instance.cashPile.transform.childCount; i++)
        // {
        //     Transform cashTransform = UiManager.Instance.cashPile.transform.GetChild(i);
        //     float animationDelay = delay;
        //     
        //     cashTransform.DOLocalMove(splitPositions[i], 0.5f).OnComplete(() =>
        //     {
        //         cashTransform.DOLocalMove(new Vector3(153f, 868f, 0f), 0.3f).SetEase(Ease.OutSine).SetDelay(animationDelay);
        //         cashTransform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.Linear).SetDelay(animationDelay);
        //     });
        //     delay += 0.1f;
        // }
        Invoke(nameof(UpdateTotalCash), .5f);
    }

    private void UpdateTotalCash()
    {
        UiManager.Instance.shouldUpdateTotalCash = true;
        UiManager.Instance.currentCashAmount = StorageManager.GetTotalScore();
        UiManager.Instance.targetCashAmount = StorageManager.GetTotalScore() + StorageManager.Instance.currentLevelScore * _multiplier;
        UiManager.Instance.incrementAmount = (UiManager.Instance.targetCashAmount - UiManager.Instance.currentCashAmount) / 1.5f;
        StorageManager.SetTotalScore(UiManager.Instance.targetCashAmount);
    }
}
