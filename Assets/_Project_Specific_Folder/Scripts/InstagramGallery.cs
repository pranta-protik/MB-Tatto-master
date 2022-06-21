using System.Collections;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstagramGallery : MonoBehaviour
{
    public GameObject pictureFramePrefab;
    private Scrollbar _scrollbar;
    private Transform _contentTransform;
    private bool _isDisplayed;
    private GameObject _lastPictureFrame;
    private bool _isScrollbarValueSet;
    private bool _isScrollingComplete;
    private bool _hasScrolledToNextPage;


    private IEnumerator Start()
    {
        _contentTransform = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        _scrollbar = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Scrollbar>();

        int totalPhotos = PlayerPrefs.GetInt("SnapshotsTaken", 0);
        transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(totalPhotos.ToString());

        SpawnPictureFrames(0, totalPhotos);

        int blankPhotos = (9 * (((totalPhotos - 1) / 9) + 1)) - totalPhotos;

        SpawnBlankPictureFrames(blankPhotos);

        yield return null;

        _hasScrolledToNextPage = true;
        _isScrollingComplete = false;
        
        if (totalPhotos > 9 && totalPhotos % 9 == 1)
        {
            _hasScrolledToNextPage = false;
            _scrollbar.value = 1 / Mathf.Floor((float) (totalPhotos - 1) / 9);
        }
        else
        {
            _scrollbar.value = 0f;
        }
        
        _isScrollbarValueSet = true;
    }

    private void SpawnPictureFrames(int startIndex, int totalPhotos)
    {
        for (int i = startIndex; i < totalPhotos; i++)
        {
            GameObject pictureFrameObj = Instantiate(pictureFramePrefab, _contentTransform.position, Quaternion.identity, _contentTransform);

            string filename = $"{Application.persistentDataPath}/Snapshots/" + (i + 1) + ".png";

            byte[] savedSnapshot = File.ReadAllBytes(filename);
            Texture2D loadedTexture = new Texture2D(720, 720, TextureFormat.ARGB32, false);
            loadedTexture.LoadImage(savedSnapshot);

            pictureFrameObj.transform.GetChild(0).gameObject.SetActive(false);
            pictureFrameObj.transform.GetChild(1).GetComponent<RawImage>().texture = loadedTexture;
            pictureFrameObj.transform.GetChild(1).gameObject.SetActive(true);

            if (i == totalPhotos - 1)
            {
                _lastPictureFrame = pictureFrameObj;
                _lastPictureFrame.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }
    }

    private void SpawnBlankPictureFrames(int frameNo)
    {
        for (int j = 0; j < frameNo; j++)
        {
            Instantiate(pictureFramePrefab, _contentTransform.position, Quaternion.identity, _contentTransform);
        }
    }

    private void Update()
    {
        if (_isScrollbarValueSet)
        {
            if (!_isScrollingComplete)
            {
                if (!_hasScrolledToNextPage)
                {
                    _scrollbar.value -= Time.deltaTime;
                }
                else
                {
                    _scrollbar.value = 0;
                }
            
                if (_scrollbar.value <= 0)
                {
                    _hasScrolledToNextPage = true;
                
                    if (!_isDisplayed)
                    {
                        _lastPictureFrame.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).OnComplete(() =>
                        {
                            UiManager.Instance.isInstagramGalleryPhotoUpdated = true;
                            transform.GetChild(3).gameObject.SetActive(false);
                            _isScrollingComplete = true;
                        });
                        _isDisplayed = true;
                    }
                }
            }
        }
    }
}
