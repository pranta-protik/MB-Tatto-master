using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeRemaining;

    private void Start()
    {
        timeRemaining = PlayerPrefs.GetFloat("ExitRemainingTime", 24 * 60 * 60f);
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
            UiManager.Instance.UpdateShopTimer($"{timeSpan.Hours:00} : {timeSpan.Minutes:00} : {timeSpan.Seconds:00}");
        }
        else
        {
            timeRemaining = 24 * 60 * 60f;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("ExitRemainingTime", timeRemaining);   
    }
}
