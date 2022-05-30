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
    public GameObject StartUI, EndUi, CompleteUI, FadeIn, UnlockPanel;
    public GameObject TapFastPanel;

    public GameObject fillbarTimer;
    public Image Timer;
    public float timerInitvalue;
    public TMP_Text TotalText, PointText , NormalCoin ; 
    
    public bool HapticsAllowed;
    public GameObject enable, disable;
    
    public TextMeshProUGUI scoreText;
    
    int currentLevel;
    public int currentLevelText;
    public override void Start()
    {
        if (scoreText != null)
        {
            scoreText.SetText(StorageManager.GetTotalCoin().ToString());
        }
        if (btnNext != null)
        {
            btnNext.onClick.AddListener(NextCallBack);   
        }
        base.Start();
        currentLevel = PlayerPrefs.GetInt("current_scene");
        currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);
        if (LevelText != null)
        {
            LevelText.text = (currentLevelText + 1).ToString();            
        }
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
        if (GameManager.Instance.levelNo == 0)
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>(). _increaseAmount = 50;
        }
        else if (GameManager.Instance.levelNo == 1)
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 50;
        }
        else
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 33;
        }
        UnlockPanel.gameObject.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Next()
    {
        DOTween.KillAll();
     //   StorageManager.Instance.SetTotalScore();
        if (currentLevel + 1 >= GameManager.Instance.LevelPrefabs.Count)
        {
            PlayerPrefs.SetInt("current_scene", 0); 
            print("reload");

        }
        else
        {
            print("next");
            PlayerPrefs.SetInt("current_scene", currentLevel + 1);

        }
        PlayerPrefs.SetInt("current_scene_text", currentLevelText + 1);
        SceneManager.LoadScene("main");
    }










}
