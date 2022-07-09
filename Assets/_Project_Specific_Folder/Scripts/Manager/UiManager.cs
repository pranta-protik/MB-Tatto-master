using System.Collections;
using UnityEngine;
using Singleton;
using TMPro;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using HomaGames.HomaBelly;

public class UiManager : Singleton<UiManager>
{
    public GameObject hurtScreen;
    public GameObject transitionScreen;
    
    public GameObject unlockPanel;
    public GameObject instagramPostPage;
    public GameObject tapFastPanel;
    public GameObject coolnessUpgradeButton;
    public GameObject valueUpgradeButton;
    public GameObject ratingScreen;

    [HideInInspector] public bool isInstagramGalleryPhotoUpdated;
    [HideInInspector] public string followerValue;
    [HideInInspector] public bool isBadTattoo;
    
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject hapticsIcon;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private GameObject priceTag;
    [SerializeField] private GameObject valueUpgradeIncrementEffect;
    [SerializeField] private GameObject mobileScreen;
    [SerializeField] private GameObject selectionMenuButton;
    [SerializeField] private GameObject shopButton;
    [SerializeField] private GameObject instagramGalleryButton;
    [SerializeField] private TextMeshProUGUI levelNoText;
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private GameObject mainMenuInstagramGallery;
    [SerializeField] private float popUpScale = 5f;
    [SerializeField] private float popUpDuration = 0.3f;
    [SerializeField] private GameObject instagramGalleryPage;
    [SerializeField] private GameObject influenceMeterPage;

    private TextMeshProUGUI _scoreText;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _currentLevelText;
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
            _scoreText.SetText(PlayerPrefs.GetInt("PriceTagBaseScore", 0).ToString());
        }
        
        _currentLevel = PlayerPrefs.GetInt("current_scene", 0);
        _currentLevelText = PlayerPrefs.GetInt("current_scene_text", 0);

        if (levelNoText != null)
        {
            levelNoText.SetText((_currentLevelText + 1).ToString());            
        }

        if (_currentLevelText < 3 && !UAManager.Instance.EnableUA)
        {
            if (shopButton != null)
            {
                shopButton.SetActive(false);
            }
        }
        
        if (_currentLevelText == 0 && !UAManager.Instance.EnableUA)
        {
            if (selectionMenuButton != null)
            {
                selectionMenuButton.SetActive(false);
            }

            if (instagramGalleryButton != null)
            {
                instagramGalleryButton.SetActive(false);
            }

            if (coolnessUpgradeButton!=null)
            {
                coolnessUpgradeButton.SetActive(false);
            }

            if (valueUpgradeButton != null)
            {
                valueUpgradeButton.SetActive(false);
            }

            if (totalScoreText != null)
            {
                totalScoreText.transform.parent.gameObject.SetActive(false);
            }
        }

        if (totalScoreText!= null)
        {
            UpdateTotalScoreText(StorageManager.GetTotalScore());
        }
    }

    private void Update()
    {
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
            isInstagramGalleryPhotoUpdated = false;
            _isFollowersUpdated = false;
            Invoke(nameof(EnableInfluenceMeterScreen), 1.5f);
        }
    }
    
    public void UpdateTotalScoreText(int totalScore)
    {
        totalScoreText.SetText("$" + totalScore);
    }

    public void OnShopButtonClick()
    {
        SceneManager.LoadScene("UpgradesShop");
    }
    
    #region Upgrade Buttons

    public void ValueUpgradeEffect(int value)
    {
        GameObject incrementTextObj =
            Instantiate(valueUpgradeIncrementEffect, priceTag.transform.GetChild(1).position, priceTag.transform.GetChild(1).rotation, priceTag.transform);
        incrementTextObj.GetComponent<TextMeshProUGUI>().SetText("+" + value);
        incrementTextObj.GetComponent<TextMeshProUGUI>().DOFade(0, 1f);
        incrementTextObj.GetComponent<RectTransform>().DOAnchorPosY(130f, 1f).OnComplete(() =>
        {
            Destroy(incrementTextObj);
        });
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
    
    #region MainMenu Buttons

    public void OnInstagramGalleryButtonClick()
    {
        mainMenuInstagramGallery.SetActive(true);
        
        // Rewarded Videos
        // Rewarded Suggested Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UPDATE_USERNAME);
    }

    public void OnInstagramGalleryCloseButtonClick()
    {
        mainMenuInstagramGallery.SetActive(false);
    }
    
    public void OnSelectionMenuButtonClick()
    {
        // Debug mode
        UAManager.Instance.isEndReached = true;
        
        levelNoText.transform.parent.gameObject.SetActive(false);
        
        // shop.SetActive(false);
        priceTag.SetActive(false);
        selectionMenuButton.SetActive(false);
        shopButton.SetActive(false);
        instagramGalleryButton.SetActive(false);
        
        _camera.transform.DOLocalRotate(new Vector3(42, 90, 0), .3f).OnComplete(() =>
        {
            selectionMenu.SetActive(true);
            selectionMenu.GetComponent<SelectionMenu>().CheckUnlockButtonTypeStatus();
            selectionMenu.GetComponent<SelectionMenu>().CheckUnlockButtonAvailability();
        });
    }
    
    public void OnCloseSelectionMenuButtonClick()
    {
        // Debug mode
        UAManager.Instance.isEndReached = false;
        
        selectionMenu.SetActive(false);
        
        _camera.transform.DOLocalRotate(new Vector3(27.761f, 90, 0), .3f).OnComplete(() =>
        {
            levelNoText.transform.parent.gameObject.SetActive(true);
            
            // shop.SetActive(true);
            priceTag.SetActive(true);
            selectionMenuButton.SetActive(true);
            shopButton.SetActive(true);
            instagramGalleryButton.SetActive(true);
        });
    }

    #endregion
    
    #region Gameplay changes

    public void ClearUIOnGameStart()
    {
        levelNoText.transform.parent.gameObject.SetActive(false);
        selectionMenuButton.SetActive(false);
        instagramGalleryButton.SetActive(false);
        shopButton.SetActive(false);
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
        
        // Rewarded Videos
        // Rewarded Suggested Event
        HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UNLOCK_POSE);
    }

    public void MovePriceTag()
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
            
            Transform postEmojiParent = instagramPostPage.transform.GetChild(1).GetChild(3);
            
            if (isBadTattoo)
            {
                postEmojiParent.GetChild(1).gameObject.SetActive(true);    
            }
            else
            {
                postEmojiParent.GetChild(0).gameObject.SetActive(true);    
            }
            
            PlayerPrefs.SetInt("TargetLikeIndex", _targetLikeIndex + 1);
            Invoke(nameof(EnableInstagramGalleryPage), 1.5f);
        }

        int leftValue = Mathf.RoundToInt(_currentLike) / 1000;
        int rightValue = Mathf.RoundToInt(_currentLike) % 1000;

        TMP_Text likeText = instagramPostPage.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        
        if (leftValue==0)
        {
            likeText.SetText(rightValue.ToString());    
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

            likeText.SetText(leftValue + "," + rightString);
        }
    }
    
    private void UpdateFollowersText()
    {
        TMP_Text followersText = instagramGalleryPage.transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>();
        
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

            followersText.SetText(Mathf.RoundToInt(_currentFollowers) + _followerValueLetter);
        }
        else
        {
            _shouldUpdateFollowersText = false;
            
            if (isBadTattoo)
            {
                Transform followerDropTextTransform = instagramGalleryPage.transform.GetChild(4).GetChild(1).GetChild(0);
                followerDropTextTransform.DOKill();
                followerDropTextTransform.gameObject.SetActive(false);
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
            
            PlayerPrefs.SetString("CurrentFollowers", followerValue);
            
            followersText.SetText(followerValue);
            _isFollowersUpdated = true;
        }
    }

    public void EnableInstagramPostPage()
    {
        instagramPostPage.SetActive(true);
        instagramPostPage.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().SetText(PlayerPrefs.GetString("Username"));
        instagramPostPage.transform.GetChild(3).GetComponent<Image>().DOFade(0, 0.5f);
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
                PlayerPrefs.SetInt("TargetFollowersIndex", _targetFollowersIndex);
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
            Transform followerDropTextTransform = instagramGalleryPage.transform.GetChild(4).GetChild(1).GetChild(0);
            followerDropTextTransform.gameObject.SetActive(true);
            followerDropTextTransform.GetComponent<TMP_Text>().DOFade(0f, 0.3f).SetLoops(-1, LoopType.Restart);
            followerDropTextTransform.GetComponent<RectTransform>().DOAnchorPosY(100f, 0.3f).SetLoops(-1, LoopType.Restart);
        }
    }

    public void EnableInfluenceMeterScreen()
    {
        influenceMeterPage.SetActive(true);
        instagramGalleryPage.SetActive(false);
    }

    public IEnumerator CrossOpponentOnInfluenceMeter()
    {
        yield return new WaitForSeconds(2f);
        influenceMeterPage.GetComponent<InfluenceMeter>().CrossOpponentVisual();
    }
    
    #endregion

    public void ReloadSceneWithNewLevel()
    {
        DOTween.KillAll();

        // Progression Events
        // Level Completed Event
        DefaultAnalytics.LevelCompleted();
        
        string levelId = (PlayerPrefs.GetInt("current_scene_text", 0) + 1).ToString();
        float levelDuration = Time.time - PlayerPrefs.GetFloat("LevelStartTime", 0);

        // Level Events
        // Level Duration Event
        HomaBelly.Instance.TrackDesignEvent("Levels:Duration:" + levelId, levelDuration);

        if (_currentLevel + 1 >= GameManager.Instance.levelPrefabs.Count)
        {
            PlayerPrefs.SetInt("current_scene", 0);
        }
        else
        {
            PlayerPrefs.SetInt("current_scene", _currentLevel + 1);
        }

        PlayerPrefs.SetInt("current_scene_text", _currentLevelText + 1);

        SceneManager.LoadScene("Main");
    }


}
