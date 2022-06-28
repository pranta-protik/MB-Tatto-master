using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInstagramGallery : MonoBehaviour
{
    public GameObject pictureFramePrefab;
    [SerializeField] private int picturesOnEachPage;
    private Transform _scrollViewContentTransform;
    private TextMeshProUGUI _postsText;
    private TextMeshProUGUI _followersText;
    
    private void Start()
    {
        _scrollViewContentTransform = transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0);
        _postsText = transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        _followersText = transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();

        int totalPhotos = PlayerPrefs.GetInt("SnapshotsTaken", 0);
        _postsText.SetText(totalPhotos.ToString());

        string currentFollowers = PlayerPrefs.GetString("CurrentFollowers");
        
        _followersText.SetText(currentFollowers == "" ? "0" : currentFollowers);

        SpawnPictureFrames(0, totalPhotos);

        int blankPhotos = (picturesOnEachPage * (((totalPhotos - 1) / picturesOnEachPage) + 1)) - totalPhotos;

        SpawnBlankPictureFrames(blankPhotos);
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
        }
    }

    private void SpawnBlankPictureFrames(int frameNo)
    {
        for (int j = 0; j < frameNo; j++)
        {
            Instantiate(pictureFramePrefab, _scrollViewContentTransform.position, Quaternion.identity, _scrollViewContentTransform);
        }
    }
}
