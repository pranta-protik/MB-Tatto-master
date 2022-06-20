using System.Collections;
using System.IO;
using UnityEngine;
using Singleton;
using TMPro;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UiManager : Singleton<UiManager>
{
    public GameObject hurtScreen;
    public GameObject transitionScreen;

    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject hapticsIcon;
    [SerializeField] private GameObject priceTag;
    [SerializeField] private GameObject mobileScreen;
    [SerializeField] private GameObject selectionMenuButton;
    [SerializeField] private GameObject shop;
    [SerializeField] private TextMeshProUGUI levelNoText;
    
    private TextMeshProUGUI _scoreText;
    private Slider _mobileScreenSlider;
    private bool _isMobileActive;
    private int _currentLevel;
    private int _currentLevelText;
        
        
    
    // public TMP_Text scoreText ;
    
    public Button btnNext;
    // public Button hand; 
    
    public GameObject EndUi, CompleteUI, UnlockPanel, ShopPnael;
    public GameObject TapFastPanel;
    public GameObject decisionScreen, cashCounter, spinnerScreen, cashPile;
    
    
    public GameObject fillbarTimer;
    public Image Timer;
    public float timerInitvalue;
    public TMP_Text TotalText ;
    
    public TMP_Text NormalCoin ;

    public bool HapticsAllowed;
    public GameObject enable, disable;

    public GameObject PopUp;
    public float popUpScale = 4.5f;
    public float popUpDuration = 0.3f;
    
    
    private new Camera _camera;
    

    public bool shouldUpdateTotalCash;
    public int targetCashAmount;
    public float currentCashAmount;
    public float incrementAmount;
    private bool _isHandAnimating;

    public GameObject[] upgradeButtons;

    public GameObject selectionMenu;

    public GameObject instaPostPage;
    public GameObject instaGalleryPage;
    public GameObject influenceMeterPage;

    private bool _shouldUpdateLikeText;
    private float _currentLike;
    private float _startLike;
    private int _targetLikeIndex;
    private int _targetLike;

    private bool _shouldUpdateFollowersText;
    private float _currentFollowers;
    private float _startFollowers;
    private int _targetFollowersIndex;
    private int _targetFollowers;

    public bool isInstaGalleryPhotoUpdated;
    public string followerValue;
    private bool _isFollowersUpdated;

    public override void Start()
    {
        base.Start();

        if (priceTag!=null)
        {
            _scoreText = priceTag.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        
        if (mobileScreen != null)
        {
            _mobileScreenSlider = mobileScreen.transform.GetChild(4).GetComponent<Slider>();
        }
        
        _currentLevel = PlayerPrefs.GetInt("current_scene", 0);
        _currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);

        if (levelNoText != null)
        {
            levelNoText.SetText((_currentLevelText + 1).ToString());            
        }
        
        if (_currentLevelText == 0)
        {
            if (shop != null)
            {
                shop.SetActive(false);
            }

            if (selectionMenuButton != null)
            {
                selectionMenuButton.SetActive(false);
            }

            if (upgradeButtons != null)
            {
                foreach (GameObject upgradeButton in upgradeButtons)
                {
                    upgradeButton.SetActive(false);
                }
            }
        }

        
        
        if (TotalText != null)
        {
            TotalText.SetText("$" + StorageManager.GetTotalScore());
        }
        if (btnNext != null)
        {
            btnNext.onClick.AddListener(NextCallBack);
        }

        _camera = Camera.main;
    }

    #region Gameplay changes

    public void ClearUIOnGameStart()
    {
        selectionMenuButton.SetActive(false);
        shop.SetActive(false);
        levelNoText.transform.parent.gameObject.SetActive(false);
        startUI.SetActive(false);
    }
    
    public void PriceTagScaleEffect()
    {
        priceTag.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).SetLoops(4, LoopType.Yoyo);
    }
    
    public void UpdatePriceTag(int value)
    {
        _scoreText.SetText(value.ToString());
        if (value < 0)
        {
            priceTag.GetComponent<Image>().DOColor(Color.red, 0.5f).SetLoops(2, LoopType.Yoyo);
        }
    }
    public void ClearUIOnFinishLine()
    {
        hapticsIcon.SetActive(false);
        priceTag.SetActive(false);
    }

    public void EnableMobileScreenUI()
    {
        mobileScreen.SetActive(true);
        transitionScreen.SetActive(false);
        mobileScreen.transform.GetChild(8).GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            mobileScreen.transform.GetChild(8).gameObject.SetActive(false);
        });
        
        int lastSnapshotNo = PlayerPrefs.GetInt("SnapshotsTaken", 0);

        if (lastSnapshotNo > 0)
        {
            string filename = $"{Application.persistentDataPath}/Snapshots/" + lastSnapshotNo + ".png";

            byte[] savedSnapshot = File.ReadAllBytes(filename);
            Texture2D loadedTexture = new Texture2D(720, 720, TextureFormat.ARGB32, false);
            loadedTexture.LoadImage(savedSnapshot);

            mobileScreen.transform.GetChild(2).GetChild(0).GetComponent<RawImage>().texture = loadedTexture;
        }
        else
        {
            mobileScreen.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        }

        mobileScreen.transform.GetChild(3).DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        mobileScreen.transform.GetChild(7).GetComponent<RectTransform>().DOAnchorPosY(340, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        _isMobileActive = true;
    }

    #endregion
    
    
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
        levelNoText.transform.parent.gameObject.SetActive(false);
        shop.SetActive(false);
        selectionMenuButton.gameObject.SetActive(false);
        _camera.transform.DOLocalRotate(new Vector3(42, 90, 0), .3f).OnComplete(() =>
        {
            selectionMenu.SetActive(true);
            selectionMenu.GetComponent<SelectionMenu>().CheckUnlockButtonAvailability();
        });
    }

    public void OnSliderClick()
    {
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).DOKill();
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).localScale = new Vector3(1f, 1f, 1f);
        mobileScreen.transform.GetChild(7).gameObject.SetActive(false);
    }
    
    public void OnCloseSelectionMenuButtonClick()
    {
        selectionMenu.SetActive(false);
        _camera.transform.DOLocalRotate(new Vector3(27.761f, 90, 0), .3f).OnComplete(() =>
        {
            levelNoText.transform.parent.gameObject.SetActive(true);
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

        if (_isMobileActive)
        {
            _camera.fieldOfView = _mobileScreenSlider.minValue + (_mobileScreenSlider.maxValue - _mobileScreenSlider.value);   
        }

        if (_shouldUpdateLikeText)
        {
            UpdateLikeText();
        }

        if (_shouldUpdateFollowersText)
        {
            UpdateFollowersText();
        }

        if (isInstaGalleryPhotoUpdated && _isFollowersUpdated)
        {
            Invoke(nameof(EnableInfluenceMeterScreen), 1f);
            isInstaGalleryPhotoUpdated = false;
            _isFollowersUpdated = false;
        }
    }

    public void TakePicture()
    {
        mobileScreen.transform.GetChild(3).DOKill();
        mobileScreen.transform.GetChild(3).localScale = new Vector3(1f, 1f, 1f);
        mobileScreen.transform.GetChild(3).GetComponent<Button>().interactable = false;
        StartCoroutine(CameraFlashEffect());
    }
    
    IEnumerator CameraFlashEffect()
    {
        _camera.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.GetChild(1).gameObject.SetActive(false);
        ScreenshotHandler.TakeScreenshot_Static(720, 720);

        mobileScreen.transform.GetChild(8).gameObject.SetActive(true);
        mobileScreen.transform.GetChild(8).GetComponent<Image>().DOColor(Color.white, 0.5f).OnComplete(() =>
        {
            mobileScreen.SetActive(false);
            instaPostPage.SetActive(true);
            instaPostPage.transform.GetChild(2).GetComponent<Image>().DOFade(0, 0.5f);
            _targetLikeIndex = PlayerPrefs.GetInt("TargetLikeIndex", 0);
            
            if (_targetLikeIndex < GameManager.Instance.likes.Count)
            {
                _targetLike = GameManager.Instance.likes[_targetLikeIndex];   
            }
            else
            {
                _targetLike = GameManager.Instance.likes[GameManager.Instance.likes.Count - 1] + Random.Range(-100, 100);
            }
            _currentLike = 0;
            _startLike = _currentLike;
            _shouldUpdateLikeText = true;
        });
    }
    
    private void UpdateLikeText()
    {
        if (_currentLike < _targetLike)
        {
            _currentLike += ((_targetLike - _startLike) / 1.5f) * Time.deltaTime;
            _currentLike = Mathf.Clamp(_currentLike, 0, _targetLike);
        }
        else
        {
            _shouldUpdateLikeText = false;
            instaPostPage.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
            instaPostPage.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(true);
            instaPostPage.transform.GetChild(1).GetChild(2).GetChild(2).gameObject.SetActive(true);
            PlayerPrefs.SetInt("TargetLikeIndex", _targetLikeIndex + 1);
            Invoke(nameof(EnableInstaGalleryPage), 1.5f);
        }

        int leftValue = Mathf.RoundToInt(_currentLike) / 1000;
        int rightValue = Mathf.RoundToInt(_currentLike) % 1000;

        if (leftValue==0)
        {
            instaPostPage.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(rightValue.ToString());    
        }
        else
        {
            string rightString;

            if (rightValue == 0)
            {
                rightString = "000";
            }
            else if (rightValue < 10)
            {
                rightString = "00" + rightValue;
            }
            else if (rightValue < 100)
            {
                rightString = "0" + rightValue;
            }
            else
            {
                rightString = rightValue.ToString();
            }

            instaPostPage.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(leftValue + "," + rightString);
        }
    }

    private string _followerValueLetter = "K";
    private void UpdateFollowersText()
    {
        if (_currentFollowers < _targetFollowers)
        {
            _currentFollowers += ((_targetFollowers - _startFollowers) / 1.5f) * Time.deltaTime;
            _currentFollowers = Mathf.Clamp(_currentFollowers, 0, _targetFollowers);

            string followerString = GameManager.Instance.followers[_targetFollowersIndex];
         
            if (followerString.EndsWith("K"))
            {
                _followerValueLetter = "K";
            }
            else if (followerString.EndsWith("M"))
            {
                _followerValueLetter = "M";
            }
            else
            {
                _followerValueLetter = "B";
            }

            instaGalleryPage.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>()
                .SetText(Mathf.RoundToInt(_currentFollowers) + _followerValueLetter);
        }
        else
        {
            _shouldUpdateFollowersText = false;
            PlayerPrefs.SetInt("TargetFollowersIndex", _targetFollowersIndex + 1);

            
            
            if (PlayerPrefs.GetInt("TargetFollowersIndex", 0) < GameManager.Instance.followers.Count)
            {
                followerValue = GameManager.Instance.followers[_targetFollowersIndex];
            }
            else
            {
                followerValue = Random.Range(899, 999) + "M";
            }
            
            instaGalleryPage.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().SetText(followerValue);
            _isFollowersUpdated = true;
        }
    }
    
    private void EnableInfluenceMeterScreen()
    {
        influenceMeterPage.SetActive(true);
        instaGalleryPage.SetActive(false);
    }
    
    private void EnableInstaGalleryPage()
    {
        instaPostPage.SetActive(false);
        instaGalleryPage.SetActive(true);

        _targetFollowersIndex = PlayerPrefs.GetInt("TargetFollowersIndex", 0);

        if (_targetFollowersIndex >= GameManager.Instance.followers.Count)
        {
            _targetFollowersIndex = GameManager.Instance.followers.Count - 1;
        }
        
        _targetFollowers = Random.Range(499, 999);
        _currentFollowers = 0;
        _startFollowers = _currentFollowers;
        _shouldUpdateFollowersText = true;
    }
    
    public void ShowPriceTag()
    {
        priceTag.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-126f, -70f), 0.5f);
    }

    public IEnumerator HurtScreenRoutine()
    {
        hurtScreen.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hurtScreen.SetActive(false);
    }
    public IEnumerator FdeDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
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
        hapticsIcon.transform.GetChild(0).gameObject.SetActive(false);
        hapticsIcon.transform.GetChild(1).gameObject.SetActive(true);
        HapticsAllowed = true;
        MMVibrationManager.SetHapticsActive(HapticsAllowed);
    }
    public void DisableHaptics()
    {
        hapticsIcon.transform.GetChild(0).gameObject.SetActive(true);
        hapticsIcon.transform.GetChild(1).gameObject.SetActive(false);
        HapticsAllowed = false;
        MMVibrationManager.SetHapticsActive(HapticsAllowed);
    }
    private void NextCallBack()
    {
        // UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        
        if (GameManager.Instance.currentLevelNo <= 23)
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>().increaseAmount = 25;
        }
        else
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>().increaseAmount = 10;
        }

        UnlockPanel.gameObject.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadSelectionMenu()
    {
         DOTween.KillAll();
        
        if (_currentLevel + 1 >= GameManager.Instance.levelPrefabs.Count)
        {
            PlayerPrefs.SetInt("current_scene", 0); 
            print("reload");

        }
        else
        {
            print("next");
            PlayerPrefs.SetInt("current_scene", _currentLevel + 1);

        }
        PlayerPrefs.SetInt("current_scene_text", _currentLevelText + 1);

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
        if (StorageManager.Instance.currentLevelScore <= 0)
        {
            // StorageManager.Instance.currentLevel = PlayerPrefs.GetInt("current_scene");
            // StorageManager.Instance.currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);
            StorageManager.Instance.currentLevelScore = 500;
        }


        // GameManager.Instance.SetTotalTime();


        if (GameManager.Instance.currentLevelNo <= 23)
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>().increaseAmount = 25;
        }
        else
        {
            UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>().increaseAmount = 10;
        }
        
        // UiManager.Instance.UnlockPanel.GetComponent<ItemCollection.GameEndUnlockItem.UnlockItemWithPercentage>()._increaseAmount = 25;
        
        decisionScreen.SetActive(false);
        spinnerScreen.SetActive(true);
        spinnerScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + StorageManager.Instance.currentLevelScore);
        spinnerScreen.transform.GetChild(5).DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void KeepTattooCallBack()
    {
        decisionScreen.SetActive(false);
        cashCounter.SetActive(false);
        
        // GameManager.Instance.SetTotalTime();
        
        // StartCoroutine(GameManager.Instance.CollsionScript.BookRoutine()); 
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
