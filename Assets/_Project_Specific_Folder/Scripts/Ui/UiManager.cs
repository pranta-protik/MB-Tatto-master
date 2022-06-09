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
    // public Button hand; 

    public TMP_Text LevelText;
    public GameObject StartUI, EndUi, CompleteUI, FadeIn, UnlockPanel, ShopPnael;
    public GameObject TapFastPanel;
    public GameObject decisionScreen, cashCounter, spinnerScreen, cashPile;
    public GameObject priceTag;
    
    public GameObject fillbarTimer;
    public Image Timer;
    public float timerInitvalue;
    public TMP_Text TotalText, PointText , NormalCoin ; 
    
    public bool HapticsAllowed;
    public GameObject enable, disable;

    public GameObject shop , PopUp;
    public float popUpScale = 4.5f;
    public float popUpDuration = 0.3f;
    
    int _currentLevel;
    private new Camera _camera;
    public int currentLevelText;

    public bool shouldUpdateTotalCash;
    public int targetCashAmount;
    public float currentCashAmount;
    public float incrementAmount;
    private bool _isHandAnimating;

    public GameObject selectionMenu;
    public Button selectionMenuButton;

    public GameObject mobileScreen;
    private Slider _mobileScreenSlider;
    public bool isMobileActive;

   
    public override void Start()
    {
        base.Start();

        _mobileScreenSlider = mobileScreen.transform.GetChild(4).GetComponent<Slider>();
        
        if (TotalText != null)
        {
            TotalText.SetText("$" + StorageManager.GetTotalCoin());
        }
        if (btnNext != null)
        {
            btnNext.onClick.AddListener(NextCallBack);
        }
        _currentLevel = PlayerPrefs.GetInt("current_scene");
        currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);

        if (LevelText != null)
        {
            LevelText.text = (currentLevelText + 1).ToString();            
        }
        
        _camera = Camera.main;

        // if (hand != null)
        // {
        //     hand.onClick.AddListener(EnableShopCallBack);
        //
        //     // Enable hand shop icon after 2nd level
        //     if (currentLevelText > 0)
        //     {
        //         hand.gameObject.SetActive(true);
        //
        //         if (currentLevelText == 1)
        //         {
        //             _isHandAnimating = true;
        //             hand.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        //         }
        //     }
        //     else
        //     {
        //         hand.gameObject.SetActive(false);
        //     }
        // }

        // Buttons handButton = ShopPnael.transform.GetChild(0).GetComponent<Buttons>();
        //
        // foreach (GameObject button in handButton.Buttonss)
        // {
        //     if (PlayerPrefs.GetInt("IsUnlockable" + button.GetComponent<ButtonCard>().handId) == 1 &&
        //         PlayerPrefs.GetInt("IsNotified" + button.GetComponent<ButtonCard>().handId) == 0)
        //     {
        //         Debug.Log("Hee");
        //         _isHandAnimating = true;
        //         hand.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        //         break;
        //     }
        // }
    }

    // private void EnableShopCallBack()
    // {
    //     hand.gameObject.SetActive(false);
    //     shop.SetActive(false);
    //
    //     if (_isHandAnimating)
    //     {
    //         DOTween.Kill(hand.transform);
    //         hand.transform.localScale = new Vector3(1f, 1f, 1f);
    //         _isHandAnimating = false;
    //     }
    //     _camera.transform.DOLocalRotate(new Vector3(42, 90, 0), .3f).OnComplete(() => { ShopPnael.SetActive(true); });
    // }

    public void OnSelectionMenuButtonClick()
    {
        shop.SetActive(false);
        selectionMenuButton.gameObject.SetActive(false);
        _camera.transform.DOLocalRotate(new Vector3(42, 90, 0), .3f).OnComplete(() =>
        {
            selectionMenu.SetActive(true);
            selectionMenu.GetComponent<SelectionMenu>().CheckUnlockButtonAvailability();
        });
    }

    public void OnCloseSelectionMenuButtonClick()
    {
        selectionMenu.SetActive(false);
        _camera.transform.DOLocalRotate(new Vector3(27.761f, 90, 0), .3f).OnComplete(() =>
        {
            shop.SetActive(true);
            selectionMenuButton.gameObject.SetActive(true);
        });
    }

    
    private void Update()
    {
        if (shouldUpdateTotalCash)
        {
            if (currentCashAmount < targetCashAmount)
            {
                currentCashAmount += Time.unscaledDeltaTime * incrementAmount;
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

        if (isMobileActive)
        {
            _camera.fieldOfView = _mobileScreenSlider.minValue + (_mobileScreenSlider.maxValue - _mobileScreenSlider.value);   
        }
    }

    public void ShowPriceTag()
    {
        priceTag.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-123f, -65f), 0.5f);
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
          PopUp.transform.GetChild(0).DOScale(new Vector3(popUpScale, popUpScale, popUpScale), popUpDuration);
    }
    public void ClosePopUps()
    {
        PopUp.transform.GetChild(0).DOScale(new Vector3(0f, 0f, 0f), popUpDuration).OnComplete(() =>
        {
            PopUp.SetActive(false);
        });
    }

    public void UpdateShopTimer(string timeLeftText)
    {
        shop.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(timeLeftText);
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
        UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        
        // if (GameManager.Instance.levelNo <= 3)
        // {
        //     UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        // }
        // else if (GameManager.Instance.levelNo > 3)
        // {
        //     UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 17;
        // }

        UnlockPanel.gameObject.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadSelectionMenu()
    {
         DOTween.KillAll();
        
        if (_currentLevel + 1 >= GameManager.Instance.LevelPrefabs.Count)
        {
            PlayerPrefs.SetInt("current_scene", 0); 
            print("reload");

        }
        else
        {
            print("next");
            PlayerPrefs.SetInt("current_scene", _currentLevel + 1);

        }
        PlayerPrefs.SetInt("current_scene_text", currentLevelText + 1);

        SceneManager.LoadScene("main");
        //   StorageManager.Instance.SetTotalScore();
    }

    public void Next()
    {
        SceneManager.LoadScene("main");   
    }

    public void SellTattoo()
    {
        cashCounter.SetActive(true);
        if (StorageManager.Instance.RewardValue <= 0)
        {
            StorageManager.Instance.currentLevel = PlayerPrefs.GetInt("current_scene");
            StorageManager.Instance.currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);
            StorageManager.Instance.RewardValue = 500;
        }


        GameManager.Instance.SetTotalTime();


        // if (GameManager.Instance.levelNo <= 3)
        // {
        //     UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        // }
        // else if (GameManager.Instance.levelNo > 3)
        // {
        //     UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 17;
        // }
        
        UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        
        decisionScreen.SetActive(false);
        spinnerScreen.SetActive(true);
        spinnerScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + StorageManager.Instance.RewardValue);
        spinnerScreen.transform.GetChild(5).DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void KeepTattooCallBack()
    {
        decisionScreen.SetActive(false);
        cashCounter.SetActive(false);
        
        GameManager.Instance.SetTotalTime();
        
        StartCoroutine(GameManager.Instance.CollsionScript.BookRoutine()); 
        //GameManager.Instance.bossWall.DOMoveY(-1.5f, 1f).OnComplete(() =>
        //{
        //    GameManager.Instance.p.enabled = true;

        //    GameManager.Instance.CollsionScript.c.enabled = true;
        //    GameManager.Instance.CollsionScript.c1.enabled = true; 
        //});

    }

    public void SpinWheel()
    {
        spinnerScreen.transform.GetChild(5).DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.1f).OnComplete(() =>
        {
            DOTween.KillAll();
            spinnerScreen.transform.GetChild(3).GetComponent<Wheel>().startSpinning = true;
            spinnerScreen.transform.GetChild(5).GetComponent<Button>().interactable = false;
        });
    }
}
