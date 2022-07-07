using System;
using UnityEngine;
using Singleton;

public class StorageManager : Singleton<StorageManager>
{

    public static int GetTotalScore() => PlayerPrefs.GetInt("LifeTimeScore", 0);

    public static void SetTotalScore(int score)
    {
        PlayerPrefs.SetInt("LifeTimeScore", score);
        CurrentScoreChangedAction?.Invoke(score);
    }

    [HideInInspector] public int currentLevelScore;

    public static Action<int> CurrentScoreChangedAction;

    public override void Start()
    {
        base.Start();
        SetCurrentScore(PlayerPrefs.GetInt("PriceTagBaseScore", 0));
    }

    public int GetCurrentScore()
    {
        return currentLevelScore;
    }
    
    public void SetCurrentScore(int score)
    {
        currentLevelScore = score;
    }
}
