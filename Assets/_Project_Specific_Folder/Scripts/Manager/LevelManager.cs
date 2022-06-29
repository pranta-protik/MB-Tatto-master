using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using HomaGames.HomaBelly;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string defaultUsername;
    // Start is called before the first frame update
    private int _gameOpenCount;
    
    private void Awake()
    { 

        GameAnalytics.Initialize();
        
        if (PlayerPrefs.GetInt("Played", 0) == 0)
        {
            PlayerPrefs.SetInt("SavedTattooNo", 0);
            _gameOpenCount = 0;
            SceneManager.LoadScene("Splash");
            PlayerPrefs.SetInt("Played", 1);
            PlayerPrefs.SetString("Username", defaultUsername);
            
            // Session
            // Game Launched Event
            HomaBelly.Instance.TrackDesignEvent("GameLaunched");
        }
        else
        {
            _gameOpenCount = PlayerPrefs.GetInt("GameOpenCount");
            LoadLastScene();
        }

        _gameOpenCount += 1;
        PlayerPrefs.SetInt("GameOpenCount", _gameOpenCount);
        
        PlayerPrefs.SetFloat("StartTime", Time.time);
        
        // Session
        // Session Started Event
        if (_gameOpenCount < 100)
        {
            HomaBelly.Instance.TrackDesignEvent("Session:"+_gameOpenCount+":Started", Time.time);    
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
