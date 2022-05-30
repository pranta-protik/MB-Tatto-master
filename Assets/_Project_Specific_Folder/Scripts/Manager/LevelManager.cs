using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int gameOpenCount;
    
    private void Awake()
    { 

        GameAnalytics.Initialize();
        gameOpenCount++;
        PlayerPrefs.SetInt("GameOpenCount", gameOpenCount);
        
        if (PlayerPrefs.GetInt("Played", 0) == 0)
        {
            SceneManager.LoadScene("Main");
            PlayerPrefs.SetInt("Played", 1);
        }
        else
            LoadLastScene();
    }
    
    public static void LoadLastScene()
    {
        SceneManager.LoadScene("SelectionMenu");

    }

    public static void LoadLevelCount()
    {
        PlayerPrefs.GetInt("current_scene");

    }
}
