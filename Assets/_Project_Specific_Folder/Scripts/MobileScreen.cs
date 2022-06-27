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
    private GameObject _watchAdButton;
    private Image _foregroundImage;
    private RawImage _lastCapturedImage;
    private bool _isMobileActive;
    private Slider _mobileScreenSlider;
    private FilterManager _filterManager;
    private bool _isPosing;

    private void Start()
    {
        _camera = Camera.main;
        _captureButton = transform.GetChild(3).gameObject;
        _watchAdButton = transform.GetChild(4).gameObject;
        _filterManager = transform.GetChild(2).GetChild(5).GetComponent<FilterManager>();
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
        _watchAdButton.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        transform.GetChild(8).GetComponent<RectTransform>().DOAnchorPosY(545f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
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
    
    public void OnWatchAdButtonClick()
    {
        Debug.Log("Ad Watched");
        PlayerPrefs.SetInt("FilterAdWatched" + _filterManager.currentFilterButtonId, 1);
        _captureButton.SetActive(true);
        _watchAdButton.SetActive(false);
        OnCaptureButtonClick();
    }
}
