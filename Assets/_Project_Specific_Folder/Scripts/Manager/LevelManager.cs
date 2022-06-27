using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using HomaGames.HomaBelly;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int gameOpenCount;
    
    private void Awake()
    { 

        GameAnalytics.Initialize();
        
        if (PlayerPrefs.GetInt("Played", 0) == 0)
        {
            PlayerPrefs.SetInt("SavedTattooNo", 0);
            gameOpenCount = 0;
            SceneManager.LoadScene("Splash");
            PlayerPrefs.SetInt("Played", 1);
            
            // Session
            // Game Launched Event
            HomaBelly.Instance.TrackDesignEvent("GameLaunched");
        }
        else
        {
            gameOpenCount = PlayerPrefs.GetInt("GameOpenCount");
            LoadLastScene();
        }

        gameOpenCount += 1;
        PlayerPrefs.SetInt("GameOpenCount", gameOpenCount);
        
        PlayerPrefs.SetFloat("StartTime", Time.time);
        
        // Session
        // Session Started Event
        if (gameOpenCount < 100)
        {
            HomaBelly.Instance.TrackDesignEvent("Session:"+gameOpenCount+":Started", Time.time);    
        }
    }
    
    private static void LoadLastScene()
    {
        SceneManager.LoadScene("Main");

    }

    public static void LoadLevelCount()
    {
        PlayerPrefs.GetInt("current_scene");
    }
}
