using System.Collections;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstagramGallery : MonoBehaviour
{
    public GameObject pictureFramePrefab;
    [SerializeField] private int picturesOnEachPage;
    private Scrollbar _scrollbar;
    private Transform _scrollViewContentTransform;
    private bool _isDisplayed;
    private GameObject _lastPictureFrame;
    private TMP_Text _postsText;
    private TMP_Text _usernameText;
    private bool _isScrollbarValueSet;
    private bool _isScrollingComplete;
    private bool _hasScrolledToNextPage;


    private IEnumerator Start()
    {
        _scrollViewContentTransform = transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0);
        _scrollbar = transform.GetChild(5).GetChild(0).GetChild(1).GetComponent<Scrollbar>();
        _postsText = transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();
        _usernameText = transform.GetChild(2).GetComponent<TMP_Text>();

        int totalPhotos = PlayerPrefs.GetInt("SnapshotsTaken", 0);
        _postsText.SetText(totalPhotos.ToString());
        
        _usernameText.SetText(PlayerPrefs.GetString("Username"));

        SpawnPictureFrames(0, totalPhotos);

        int blankPhotos = (picturesOnEachPage * (((totalPhotos - 1) / picturesOnEachPage) + 1)) - totalPhotos;

        SpawnBlankPictureFrames(blankPhotos);

        yield return null;

        _hasScrolledToNextPage = true;
        _isScrollingComplete = false;
        
        if (totalPhotos > picturesOnEachPage && totalPhotos % picturesOnEachPage == 1)
        {
            _hasScrolledToNextPage = false;
            _scrollbar.value = 1 / Mathf.Floor((float) (totalPhotos - 1) / picturesOnEachPage);
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
            GameObject pictureFrameObj = Instantiate(pictureFramePrefab, _scrollViewContentTransform.position, Quaternion.identity, _scrollViewContentTransform);

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
            Instantiate(pictureFramePrefab, _scrollViewContentTransform.position, Quaternion.identity, _scrollViewContentTransform);
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
                            _isScrollingComplete = true;
                        });
                        _isDisplayed = true;
                    }
                }
            }
        }
    }
}
