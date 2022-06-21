using System.Collections;
using System.IO;
using UnityEngine;
using Singleton;
using TMPro;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class UiManager : Singleton<UiManager>
{
    public GameObject hurtScreen;
    public GameObject transitionScreen;
    public TextMeshProUGUI totalScoreText;
    public GameObject unlockPanel;
    public GameObject instagramPostPage;
    // public GameObject tapFastPanel;
    
    [HideInInspector] public bool isInstagramGalleryPhotoUpdated;
    [HideInInspector] public string followerValue;
    [HideInInspector] public bool isBadTattoo;
    
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject hapticsIcon;
    [SerializeField] private GameObject priceTag;
    [SerializeField] private GameObject mobileScreen;
    [SerializeField] private GameObject selectionMenuButton;
    [SerializeField] private GameObject shop;
    [SerializeField] private TextMeshProUGUI levelNoText;
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private float popUpScale = 5f;
    [SerializeField] private float popUpDuration = 0.3f;
    [SerializeField] private GameObject instagramGalleryPage;
    [SerializeField] private GameObject influenceMeterPage;

    private TextMeshProUGUI _scoreText;
    private Slider _mobileScreenSlider;
    private bool _isMobileActive;
    private int _currentLevel;
    private int _currentLevelText;
    private bool _isHapticsAllowed;
    private Camera _camera;
    private string _followerValueLetter = "K";
    
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
    private bool _isFollowersUpdated;

    public override void Start()
    {
        base.Start();

        _camera = Camera.main;
        
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

        if (totalScoreText!= null)
        {
            totalScoreText.SetText("$" + StorageManager.GetTotalScore());
        }
    }
    
    private void Update()
    {
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

        if (isInstagramGalleryPhotoUpdated && _isFollowersUpdated)
        {
            instagramGalleryPage.transform.GetChild(2).gameObject.SetActive(true);
            instagramGalleryPage.transform.GetChild(2).DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            // Invoke(nameof(EnableInfluenceMeterScreen), 1f);
            isInstagramGalleryPhotoUpdated = false;
            _isFollowersUpdated = false;
        }
    }

    #region PopUps

    public void OnShopPopUpButtonClick()
    {
        shop.transform.GetChild(1).gameObject.SetActive(true);
        shop.transform.GetChild(2).gameObject.SetActive(true);
        shop.transform.GetChild(2).DOScale(new Vector3(popUpScale, popUpScale, popUpScale), popUpDuration);
    }

    public void OnClosePopUpButtonClick()
    {
        shop.transform.GetChild(2).DOScale(new Vector3(0f, 0f, 0f), popUpDuration).OnComplete(() =>
        {
            shop.transform.GetChild(1).gameObject.SetActive(false);
            shop.transform.GetChild(2).gameObject.SetActive(false); 
        });
    }

    public void UpdateShopTimer(string timeLeftText)
    {
        shop.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(timeLeftText);
    }
    
    #endregion

    #region Haptics

    public void OnEnableHapticsButtonClick()
    {
        hapticsIcon.transform.GetChild(0).gameObject.SetActive(false);
        hapticsIcon.transform.GetChild(1).gameObject.SetActive(true);
        _isHapticsAllowed = true;
        MMVibrationManager.SetHapticsActive(_isHapticsAllowed);
    }
    public void OnDisableHapticsButtonClick()
    {
        hapticsIcon.transform.GetChild(0).gameObject.SetActive(true);
        hapticsIcon.transform.GetChild(1).gameObject.SetActive(false);
        _isHapticsAllowed = false;
        MMVibrationManager.SetHapticsActive(_isHapticsAllowed);
    }

    #endregion

    #region SelectionMenu

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

    #endregion
    
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

    #endregion

    #region Camera Settings

    public void OnSliderClick()
    {
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).DOKill();
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).localScale = new Vector3(1f, 1f, 1f);
        mobileScreen.transform.GetChild(7).gameObject.SetActive(false);
    }
    
    public void OnCaptureButtonClick()
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
            instagramPostPage.SetActive(true);
            instagramPostPage.transform.GetChild(2).GetComponent<Image>().DOFade(0, 0.5f);
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

    #endregion

    #region Instagram Like and Follower Settings
    
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
            if (isBadTattoo)
            {
                instagramPostPage.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(true);    
            }
            else
            {
                instagramPostPage.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);    
            }
            PlayerPrefs.SetInt("TargetLikeIndex", _targetLikeIndex + 1);
            Invoke(nameof(EnableInstagramGalleryPage), 1.5f);
        }

        int leftValue = Mathf.RoundToInt(_currentLike) / 1000;
        int rightValue = Mathf.RoundToInt(_currentLike) % 1000;

        if (leftValue==0)
        {
            instagramPostPage.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(rightValue.ToString());    
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

            instagramPostPage.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(leftValue + "," + rightString);
        }
    }
    
    private void UpdateFollowersText()
    {
        if (_currentFollowers < _targetFollowers)
        {
            _currentFollowers += ((_targetFollowers - _startFollowers) / 1.5f) * Time.deltaTime;
            _currentFollowers = Mathf.Clamp(_currentFollowers, 0, _targetFollowers);

            FollowerInfoSet followerInfoSet = GameManager.Instance.followers[_targetFollowersIndex];

            if (followerInfoSet.scale == "K")
            {
                _followerValueLetter = "K";
            }
            else if (followerInfoSet.scale == "M")
            {
                _followerValueLetter = "M";
            }
            else
            {
                _followerValueLetter = "B";
            }

            instagramGalleryPage.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>()
                .SetText(Mathf.RoundToInt(_currentFollowers) + _followerValueLetter);
        }
        else
        {
            _shouldUpdateFollowersText = false;
            if (isBadTattoo)
            {
                instagramGalleryPage.transform.GetChild(1).GetChild(2).GetChild(0).DOKill();
                instagramGalleryPage.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(false);    
            }

            if (PlayerPrefs.GetInt("TargetFollowersIndex", 0) <= GameManager.Instance.followers.Count)
            {
                int randomRange = GameManager.Instance.followers[_targetFollowersIndex].randomRange;
                
                int totalFollowers;
                
                if (isBadTattoo)
                {
                    totalFollowers = GameManager.Instance.followers[_targetFollowersIndex].value + Random.Range(-randomRange, 0);
                }
                else
                {
                    totalFollowers = GameManager.Instance.followers[_targetFollowersIndex].value + Random.Range(0, randomRange);
                }
                
                followerValue = totalFollowers + GameManager.Instance.followers[_targetFollowersIndex].scale;
            }
            else
            {
                followerValue = Random.Range(899, 999) + "M";
                PlayerPrefs.SetInt("TargetFollowersIndex", GameManager.Instance.followers.Count);
            }
            
            instagramGalleryPage.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().SetText(followerValue);
            _isFollowersUpdated = true;
        }
    }

    private void EnableInstagramGalleryPage()
    {
        instagramPostPage.SetActive(false);
        instagramGalleryPage.SetActive(true);

        _targetFollowersIndex = PlayerPrefs.GetInt("TargetFollowersIndex", 0);

        if (isBadTattoo)
        {
            if (_targetFollowersIndex > 1)
            {
                _targetFollowersIndex -= 2;
                PlayerPrefs.SetInt("TargetFollowersIndex", _targetFollowersIndex + 1);
            }
            else
            {
                _targetFollowersIndex = 0;
                PlayerPrefs.SetInt("TargetFollowersIndex", _targetFollowersIndex + 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("TargetFollowersIndex", _targetFollowersIndex + 1);
        }

        if (_targetFollowersIndex >= GameManager.Instance.followers.Count)
        {
            _targetFollowersIndex = GameManager.Instance.followers.Count - 1;
        }

        _targetFollowers = Random.Range(499, 999);
        _currentFollowers = 0;
        _startFollowers = _currentFollowers;
        _shouldUpdateFollowersText = true;

        if (isBadTattoo)
        {
            instagramGalleryPage.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
            instagramGalleryPage.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0f, 0.3f).SetLoops(-1, LoopType.Restart);
            instagramGalleryPage.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(100f, 0.3f)
                .SetLoops(-1, LoopType.Restart);
        }
    }

    public void EnableInfluenceMeterScreen()
    {
        influenceMeterPage.SetActive(true);
        instagramGalleryPage.SetActive(false);
    }
    
    #endregion
    
    public void ReloadSceneWithNewLevel()
    {
        DOTween.KillAll();
        
        if (_currentLevel + 1 >= GameManager.Instance.levelPrefabs.Count)
        {
            PlayerPrefs.SetInt("current_scene", 0); 
            // print("reload");
        }
        else
        {
            // print("next");
            PlayerPrefs.SetInt("current_scene", _currentLevel + 1);
        }
        PlayerPrefs.SetInt("current_scene_text", _currentLevelText + 1);

        SceneManager.LoadScene("Main");
    }
}
