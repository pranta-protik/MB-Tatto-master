using System;
using System.Collections;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MobileScreen : MonoBehaviour
{
    private Camera _camera;
    private bool _isMobileActive;
    private Slider _mobileScreenSlider;
    private bool _isPosing;

    private void Start()
    {
        _camera = Camera.main;
        _mobileScreenSlider = transform.GetChild(4).GetComponent<Slider>();
        
        transform.GetChild(8).GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            transform.GetChild(8).gameObject.SetActive(false);
        });
        
        int lastSnapshotNo = PlayerPrefs.GetInt("SnapshotsTaken", 0);

        if (lastSnapshotNo > 0)
        {
            string filename = $"{Application.persistentDataPath}/Snapshots/" + lastSnapshotNo + ".png";

            byte[] savedSnapshot = File.ReadAllBytes(filename);
            Texture2D loadedTexture = new Texture2D(720, 720, TextureFormat.ARGB32, false);
            loadedTexture.LoadImage(savedSnapshot);

            transform.GetChild(2).GetChild(0).GetComponent<RawImage>().texture = loadedTexture;
        }
        else
        {
            transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        }

        transform.GetChild(3).DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        transform.GetChild(7).GetComponent<RectTransform>().DOAnchorPosY(340, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
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
        transform.GetChild(7).gameObject.SetActive(false);
    }
    
    public void OnCaptureButtonClick()
    {
        transform.GetChild(3).DOKill();
        transform.GetChild(3).localScale = new Vector3(1f, 1f, 1f);
        transform.GetChild(3).GetComponent<Button>().interactable = false;
        StartCoroutine(CameraFlashEffect());
    }
    
    IEnumerator CameraFlashEffect()
    {
        _camera.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.GetChild(1).gameObject.SetActive(false);
        ScreenshotHandler.TakeScreenshot_Static();

        transform.GetChild(8).gameObject.SetActive(true);
        transform.GetChild(8).GetComponent<Image>().DOColor(Color.white, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _isMobileActive = false;
            UiManager.Instance.EnableInstagramPostPage();
        });
    }
    
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
}
