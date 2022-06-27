using UnityEngine;
using Singleton;
using UnityEngine.SceneManagement;


public class SceneLoadCounter : Singleton<SceneLoadCounter>
{
    public int SceneLoadCount;
    
    public override void Start()
    {


        base.Start();
        SceneLoadCount = PlayerPrefs.GetInt("Count", 1);
        SceneManager.sceneLoaded += IncrementSceneLoad;

    }

    private void IncrementSceneLoad(Scene scene, LoadSceneMode mode)
    {

        if (SceneManager.GetActiveScene().buildIndex == SceneLoadCount)
            return;
        else
        {
            SceneLoadCount++;
            PlayerPrefs.SetInt("Count", SceneLoadCount);
        }

    }


}
