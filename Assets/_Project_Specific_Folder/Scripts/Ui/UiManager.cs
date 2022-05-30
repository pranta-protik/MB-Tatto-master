using System;
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
    public GameObject decisionScreen, cashCounter, spinnerScreen, cashPile;

    public GameObject fillbarTimer;
    public Image Timer;
    public float timerInitvalue;
    public TMP_Text TotalText, PointText , NormalCoin ; 
    
    public bool HapticsAllowed;
    public GameObject enable, disable;

    public GameObject shop , PopUp;
    public float popUpScale = 4.5f;
    public float popUpDuration = 0.3f;
    
    int currentLevel;
    public int currentLevelText;

    public bool shouldUpdateTotalCash;
    public int targetCashAmount;
    public float currentCashAmount;
    
    public override void Start()
    {
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

    private void Update()
    {
        if (shouldUpdateTotalCash)
        {
            if (currentCashAmount < targetCashAmount)
            {
                currentCashAmount += Time.unscaledDeltaTime * (targetCashAmount / 3f);
                currentCashAmount = Mathf.Clamp(currentCashAmount, 0, targetCashAmount);
                cashCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + Mathf.RoundToInt(currentCashAmount));
            }
            else
            {
                cashPile.SetActive(false);
                UnlockPanel.SetActive(true);
                shouldUpdateTotalCash = false;
            }
        }    
    }

    public IEnumerator FdeDelayRoutine()
    {
        FadeIn.SetActive(true);
        yield return new WaitForSeconds(.3f);
        FadeIn.SetActive(false);
     
    }
    public void PopUps()
    {
          PopUp.SetActive(true);
           PopUp.transform. DOScale(new Vector3(popUpScale, popUpScale, popUpScale), popUpDuration);
    }
    public void ClosePopUps()
    {
        PopUp.transform.DOScale(new Vector3(0f, 0f, 0f), popUpDuration).OnComplete(() =>
        {
            PopUp.transform.gameObject.SetActive(false);
       
        });
    }
    public void ShopPopUp()
    {
        shop.transform.GetChild(1).gameObject.SetActive(true);
        shop.transform.GetChild(2).gameObject.SetActive(true);
        shop.transform.GetChild(2).DOScale(new Vector3(popUpScale, popUpScale, popUpScale), popUpDuration);
    }

    public void ClosePopUp()
    {
        shop.transform.GetChild(2).DOScale(new Vector3(0f, 0f, 0f), popUpDuration).OnComplete(() =>
        {
            shop.transform.GetChild(1).gameObject.SetActive(false);
            shop.transform.GetChild(2).gameObject.SetActive(false); 
        });
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
    public void LoadSelectionMenu()
    {
        DOTween.KillAll();
        
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
        
        SceneManager.LoadScene("SelectionMenu");
     //   StorageManager.Instance.SetTotalScore();
    }

    public void Next()
    {
        SceneManager.LoadScene("main");   
    }

    public void SellTattoo()
    {
        decisionScreen.SetActive(false);
        spinnerScreen.SetActive(true);
        spinnerScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + StorageManager.Instance.RewardValue);
    }

    public void SpinWheel()
    {
        spinnerScreen.transform.GetChild(3).GetComponent<Wheel>().startSpinning = true;
        spinnerScreen.transform.GetChild(5).GetComponent<Button>().interactable = false;
    }
}
