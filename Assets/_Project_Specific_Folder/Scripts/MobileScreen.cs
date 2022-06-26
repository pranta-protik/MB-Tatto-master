using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MobileScreen : MonoBehaviour
{
    private Camera _camera;
    private GameObject _captureButton;
    private GameObject _videoButton;
    private Image _foregroundImage;
    private RawImage _lastCapturedImage;
    private bool _isMobileActive;
    private Slider _mobileScreenSlider;
    private bool _isPosing;
    private bool _isFilterActive;
    [SerializeField] private int _currentFilterButtonId;

    private void Start()
    {
        _camera = Camera.main;
        _captureButton = transform.GetChild(3).gameObject;
        _videoButton = transform.GetChild(4).gameObject;
        _mobileScreenSlider = transform.GetChild(5).GetComponent<Slider>();
        _foregroundImage = transform.GetChild(9).GetComponent<Image>();
        _lastCapturedImage = transform.GetChild(2).GetChild(0).GetComponent<RawImage>();

        _foregroundImage.DOFade(0f, 0.5f).OnComplete(() =>
        {
            _foregroundImage.gameObject.SetActive(false);
        });
        
        int lastSnapshotNo = PlayerPrefs.GetInt("SnapshotsTaken", 0);

        if (lastSnapshotNo > 0)
        {
            string filename = $"{Application.persistentDataPath}/Snapshots/" + lastSnapshotNo + ".png";

            byte[] savedSnapshot = File.ReadAllBytes(filename);
            Texture2D loadedTexture = new Texture2D(720, 720, TextureFormat.ARGB32, false);
            loadedTexture.LoadImage(savedSnapshot);

            _lastCapturedImage.texture = loadedTexture;
        }
        else
        {
            _lastCapturedImage.gameObject.SetActive(false);
        }

        _captureButton.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        _videoButton.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        transform.GetChild(8).GetComponent<RectTransform>().DOAnchorPosY(340, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);

        _isMobileActive = true;
    }

    private void Update()
    {
        if (_isMobileActive)
        {
            _camera.fieldOfView = _mobileScreenSlider.minValue + (_mobileScreenSlider.maxValue - _mobileScreenSlider.value);   
        }
    }
    
    public void OnSliderClick()
    {
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).DOKill();
        _mobileScreenSlider.transform.GetChild(2).GetChild(0).localScale = new Vector3(1f, 1f, 1f);
        transform.GetChild(8).gameObject.SetActive(false);
    }
    
    public void OnCaptureButtonClick()
    {
        _captureButton.transform.DOKill();
        _captureButton.transform.localScale = new Vector3(1f, 1f, 1f);
        _captureButton.transform.GetComponent<Button>().interactable = false;
        StartCoroutine(CameraFlashEffect());
    }

    IEnumerator CameraFlashEffect()
    {
        _camera.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.GetChild(1).gameObject.SetActive(false);
        ScreenshotHandler.TakeScreenshot_Static();

        _foregroundImage.gameObject.SetActive(true);
        _foregroundImage.DOColor(Color.white, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _isMobileActive = false;
            UiManager.Instance.EnableInstagramPostPage();
        });
    }

    #region Filter

    public void WatchAd()
    {
        PlayerPrefs.SetInt("AdWatched" + _currentFilterButtonId, 1);
        _videoButton.SetActive(false);
        _captureButton.SetActive(true);
        OnCaptureButtonClick();
    }
    
    public void Filter1Effect(GameObject filterButtonObj)
    {
        FilterButton filterButton = filterButtonObj.GetComponent<FilterButton>();
        _currentFilterButtonId = filterButton.buttonId;
        
        if (!_isFilterActive)
        {
            _isFilterActive = true;
            _captureButton.SetActive(true);
            _videoButton.SetActive(false);
        }
        else
        {
            _isFilterActive = false;
            _captureButton.SetActive(true);
            _videoButton.SetActive(false);
        }
    }

    public void Filter2Effect(GameObject filterButtonObj)
    {
        FilterButton filterButton = filterButtonObj.GetComponent<FilterButton>();
        _currentFilterButtonId = filterButton.buttonId;
        
        if (!_isFilterActive)
        {
            _isFilterActive = true;
            if (PlayerPrefs.GetInt("AdWatched" + _currentFilterButtonId, 0) == 0)
            {
                _captureButton.SetActive(false);
                _videoButton.SetActive(true);    
            }
            else
            {
                _captureButton.SetActive(true);
                _videoButton.SetActive(false);    
            }
        }
        else
        {
            _isFilterActive = false;
            _captureButton.SetActive(true);
            _videoButton.SetActive(false);
        }
    }
    
    public void Filter3Effect(GameObject filterButtonObj)
    {
        FilterButton filterButton = filterButtonObj.GetComponent<FilterButton>();
        _currentFilterButtonId = filterButton.buttonId;
        
        if (!_isFilterActive)
        {
            _isFilterActive = true;
            if (PlayerPrefs.GetInt("AdWatched" + _currentFilterButtonId, 0) == 0)
            {
                _captureButton.SetActive(false);
                _videoButton.SetActive(true);    
            }
            else
            {
                _captureButton.SetActive(true);
                _videoButton.SetActive(false);    
            }
        }
        else
        {
            _isFilterActive = false;
            _captureButton.SetActive(true);
            _videoButton.SetActive(false);
        }
    }
    
    public void Filter4Effect(GameObject filterButtonObj)
    {
        FilterButton filterButton = filterButtonObj.GetComponent<FilterButton>();
        _currentFilterButtonId = filterButton.buttonId;
        
        if (!_isFilterActive)
        {
            _isFilterActive = true;
            if (PlayerPrefs.GetInt("AdWatched" + _currentFilterButtonId, 0) == 0)
            {
                _captureButton.SetActive(false);
                _videoButton.SetActive(true);    
            }
            else
            {
                _captureButton.SetActive(true);
                _videoButton.SetActive(false);    
            }
        }
        else
        {
            _isFilterActive = false;
            _captureButton.SetActive(true);
            _videoButton.SetActive(false);
        }
    }

    #endregion
    
    #region Hand Pose
    
    public void PlayFingerPose()
    {
        if (_isPosing)
        {
            _isPosing = false;
            GameManager.Instance.ResetPose();
        }
        else
        {
            _isPosing = true;
            GameManager.Instance.PlayPoseAnimation(0);   
        }
    }

    public void PlayHopePose()
    {
        if (_isPosing)
        {
            _isPosing = false;
            GameManager.Instance.ResetPose();
        }
        else
        {
            _isPosing = true;
            GameManager.Instance.PlayPoseAnimation(1);   
        }
    }

    public void PlayPeacePose()
    {
        if (_isPosing)
        {
            _isPosing = false;
            GameManager.Instance.ResetPose();
        }
        else
        {
            _isPosing = true;
            GameManager.Instance.PlayPoseAnimation(2);
        }
    }

    public void PlayRockPose()
    {
        if (_isPosing)
        {
            _isPosing = false;
            GameManager.Instance.ResetPose();
        }
        else
        {
            _isPosing = true;
            GameManager.Instance.PlayPoseAnimation(3);   
        }
    }
    
    #endregion
}
