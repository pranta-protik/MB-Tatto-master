using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wheel : MonoBehaviour
{
    private int randomValue;
    private float timeInterval;
    private bool coroutineAllowed;
    private int finalAngle;
    private void Start()
    {
        coroutineAllowed = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Spin());
        }
    }
    
    private IEnumerator Spin()
    {
        coroutineAllowed = false;
        randomValue = Random.Range(20, 30);
        timeInterval = 0.1f;
        
        int i = 0;
        Debug.Log(randomValue);
        Debug.Log(transform.eulerAngles.z);
        while (i<randomValue)
        {
            // transform.DORotate(new Vector3(0, 0, 22.5f), timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
            transform.DORotate(new Vector3(0f, 0f, 22.5f), timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (i>Mathf.RoundToInt(randomValue * 0.5f))
                {
                    if (timeInterval < 0.1f)
                    {
                        timeInterval += Time.deltaTime;   
                    }
                }
                
                if (i > Mathf.RoundToInt(randomValue * 0.85f))
                {
                    if (timeInterval < 0.2f)
                    {
                        timeInterval += Time.deltaTime;   
                    }
                }
                i++;
                
            });
            
            yield return new WaitForSeconds(timeInterval);
        }

        if (Mathf.RoundToInt(transform.eulerAngles.z) % 45 != 0)
        {
            transform.Rotate(0f, 0f, 22.5f);
            // transform.DORotate(new Vector3(0f, 0f, 22.5f), timeInterval, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
        }

        finalAngle = Mathf.RoundToInt(transform.eulerAngles.z);
        Debug.Log(finalAngle);
        switch (finalAngle)
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

        coroutineAllowed = true;
    }
}
