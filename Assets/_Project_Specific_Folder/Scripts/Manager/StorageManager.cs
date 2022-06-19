using UnityEngine;
using Singleton;

public class StorageManager : Singleton<StorageManager>
{
    public static int GetTotalScore() => PlayerPrefs.GetInt("LifeTimeScore", 0);
    public static void SetTotalScore(int score) => PlayerPrefs.SetInt("LifeTimeScore", score);
    
    [HideInInspector] public int currentLevelScore;
    [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentLevelText;
    
    public void IncreaseScore(int count)
    {
        currentLevelScore += count;
        UiManager.Instance.scoreText.text = currentLevelScore.ToString();
    }
}
