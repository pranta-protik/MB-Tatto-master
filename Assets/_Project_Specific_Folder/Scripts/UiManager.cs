using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using TMPro;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UiManager : Singleton<UiManager>
{
    public Button btnNext;

    public TMP_Text LevelText;
    public GameObject StartUI, EndUi, CompleteUI , FadeIn;
    public GameObject TapFastPanel;

    public GameObject fillbarTimer;
    public Image Timer;
    public float timerInitvalue;
    public TMP_Text TotalText, PointText , NormalCoin ; 
    
    public bool HapticsAllowed;
    public GameObject enable, disable; 
    
    int currentLevel; int currentLevelText;
    public override void Start()
    {
        btnNext.onClick.AddListener(NextCallBack);
        base.Start();
        currentLevel = PlayerPrefs.GetInt("current_scene");
        LevelText.text = SceneLoadCounter.Instance.SceneLoadCount.ToString();
    }
    public IEnumerator FdeDelayRoutine()
    {
        FadeIn.SetActive(true);
        yield return new WaitForSeconds(.3f);
        FadeIn.SetActive(false);
     
    }

    public void EnableHaptics()
    {
        enable.gameObject.SetActive(false);
        disable.gameObject.SetActive(true);
        HapticsAllowed = false;
        MMVibrationManager.SetHapticsActive(HapticsAllowed);
    }
    public void DisableHaptics()
    {
        enable.gameObject.SetActive(true);
        disable.gameObject.SetActive(false);
        HapticsAllowed = true;
        MMVibrationManager.SetHapticsActive(HapticsAllowed);
    }
    private void NextCallBack()
    {
        Reset1();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Reset1()
    {


        // PlayerPrefs.SetInt("SaveScene", SceneManager.GetActiveScene().buildIndex );
        if (currentLevel + 1 >= GameManager.Instance.LevelPrefabs.Count)
        {
            PlayerPrefs.SetInt("current_scene", 2);
        }
        else
        {
            PlayerPrefs.SetInt("current_scene", currentLevel + 1);
        }

        PlayerPrefs.SetInt("current_scene_text", currentLevelText + 1);


        SceneManager.LoadScene(2);
    }
    private void NextCallBack(bool success = false)
    {
        PlayerPrefs.SetInt("current_scene_text", currentLevelText + 1);
        SceneManager.LoadScene(2);
        
    }









}
