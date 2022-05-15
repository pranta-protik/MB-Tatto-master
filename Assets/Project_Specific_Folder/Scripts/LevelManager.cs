using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
   

        if (PlayerPrefs.GetInt("Played", 0) == 0)
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("Played", 1);

        }
        else
            LoadLastScene();
    }


    public static void LoadLastScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("current_scene"), 0);

    }

    public static void LoadLevelCount()
    {
        PlayerPrefs.GetInt("current_scene");

    }
}
