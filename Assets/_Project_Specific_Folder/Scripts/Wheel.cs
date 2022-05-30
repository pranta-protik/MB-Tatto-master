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
    private bool _startSpinning;
    private int _finalAngle;


    private void Start()
    {
        _startSpinning = true;
    }

    private void Update()
    {
        if (_startSpinning)
        {
            Debug.Log("Here");
            StartCoroutine(Spin());
            _startSpinning = false;
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
            transform.DORotate(new Vector3(0f, 0f, 22.5f), _timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(() =>
            {
                _timeInterval += Time.unscaledDeltaTime * 0.1f;
                i++;

            });

            yield return new WaitForSeconds(_timeInterval);
        }

        if (Mathf.RoundToInt(transform.eulerAngles.z) % 45 != 0)
        {
            transform.DORotate(new Vector3(0f, 0f, 22.5f), _timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(GetResultValue);
        }
        else
        {
            GetResultValue();
        }
    }

    private void GetResultValue()
    {
        _finalAngle = Mathf.RoundToInt(transform.eulerAngles.z);

        switch (_finalAngle)
        {
            case 0:
                Debug.Log("1");
                break;
            case 45:
                Debug.Log("2");
                break;
            case 90:
                Debug.Log("3");
                break;
            case 135:
                Debug.Log("4");
                break;
            case 180:
                Debug.Log("5");
                break;
            case 225:
                Debug.Log("6");
                break;
            case 270:
                Debug.Log("7");
                break;
            case 315:
                Debug.Log("8");
                break;
        }
    }
}
