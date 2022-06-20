using DG.Tweening;
using UnityEngine;
using Singleton;
using UnityEngine.UI;

public class StorageManager : Singleton<StorageManager>
{
    public static int GetTotalScore() => PlayerPrefs.GetInt("LifeTimeScore", 0);
    public static void SetTotalScore(int score) => PlayerPrefs.SetInt("LifeTimeScore", score);
    
    [HideInInspector] public int currentLevelScore;
    [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentLevelText;

    public int GetCurrentScore()
    {
        return currentLevelScore;
    }
    
    public void SetCurrentScore(int score)
    {
        currentLevelScore = score;
    }
}
