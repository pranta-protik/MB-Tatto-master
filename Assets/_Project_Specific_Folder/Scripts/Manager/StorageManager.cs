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

    public void UpdateScore(int count)
    {
        currentLevelScore += count;
        UiManager.Instance.scoreText.text = currentLevelScore.ToString();
        if (count < 0)
        {
            UiManager.Instance.priceTag.GetComponent<Image>().DOColor(Color.red, 0.5f).SetLoops(2, LoopType.Yoyo);
        }
    }
}
