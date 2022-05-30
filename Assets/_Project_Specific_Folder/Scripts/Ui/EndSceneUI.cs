using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class EndSceneUI : MonoBehaviour
{
    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    public void Reset1()
    {
       // DOTween.KillAll();
       // SceneManager.LoadScene(02);
        string sceneName = currentScene.name;
        PlayerPrefs.SetInt("SaveScene", SceneManager.GetActiveScene().buildIndex);

        if (sceneName == "S Main 3")
        {
            SceneManager.LoadScene("S Main 1");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
