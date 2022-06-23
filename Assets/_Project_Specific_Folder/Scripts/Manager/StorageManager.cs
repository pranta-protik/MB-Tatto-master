using UnityEngine;
using Singleton;

public class StorageManager : Singleton<StorageManager>
{
    public static int GetTotalScore() => PlayerPrefs.GetInt("LifeTimeScore", 0);
    public static void SetTotalScore(int score) => PlayerPrefs.SetInt("LifeTimeScore", score);
    
    [HideInInspector] public int currentLevelScore;

    public override void Start()
    {
        base.Start();
        SetCurrentScore(PlayerPrefs.GetInt("BaseScore", 0));
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
