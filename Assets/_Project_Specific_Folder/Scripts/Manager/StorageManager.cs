using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
public class StorageManager : Singleton<StorageManager>
{
    [SerializeField] int TotalCoinsCount;
    public int RewardValue;
    public int TotalScore;
    public static int GetTotalCoin() => PlayerPrefs.GetInt("LifeTimeScore");
    public static void SaveTotalCoin(int amount) => PlayerPrefs.SetInt("LifeTimeScore", amount); 
    public int currentLevel;
    public int currentLevelText;
    public void SetTotalScore()
    {
        int currentLifetimeScore = PlayerPrefs.GetInt("LifeTimeScore", 0);
        int newLifeTimeScore = currentLifetimeScore + RewardValue;
        PlayerPrefs.SetInt("TotalCoinsCount", newLifeTimeScore + TotalCoinsCount);
        PlayerPrefs.SetInt("LifeTimeScore", newLifeTimeScore);
    }


    public void GetTotalScore()
    {
        TotalScore = PlayerPrefs.GetInt("LifeTimeScore");
        UiManager.Instance.TotalText.text = TotalScore.ToString("0");
    }
    public void IncreasePoints(int count)
    {
        /* currentScene = SceneManager.GetActiveScene();*/
        currentLevel = PlayerPrefs.GetInt("current_scene");
        currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);
        RewardValue += count;

  
      //  UiManager.Instance.PointText.text = RewardValue.ToString();

        //   UiManager.Instance.NormalCoin.text = RewardValue.ToString();
        // RewardMultiplyValue = RewardValue * 2;
        // Multiplied.text = i.ToString();
    }
}
