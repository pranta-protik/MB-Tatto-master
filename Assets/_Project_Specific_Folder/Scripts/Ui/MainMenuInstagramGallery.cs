using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInstagramGallery : MonoBehaviour
{
    public GameObject pictureFramePrefab;
    [SerializeField] private int picturesOnEachPage;
    private Transform _scrollViewContentTransform;
    private TMP_Text _postsText;
    private TMP_Text _followersText;
    private TMP_Text _usernameText;
    private GameObject _changeUsernamePanel;
    private TMP_InputField _usernameInputField;
    
    private void Start()
    {
        _scrollViewContentTransform = transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0);
        _postsText = transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>();
        _followersText = transform.GetChild(5).GetChild(1).GetComponent<TMP_Text>();
        _usernameText = transform.GetChild(2).GetComponent<TMP_Text>();
        
        _changeUsernamePanel = transform.GetChild(8).gameObject;
        _usernameInputField = _changeUsernamePanel.transform.GetChild(1).GetComponent<TMP_InputField>();

        int totalPhotos = PlayerPrefs.GetInt("SnapshotsTaken", 0);
        _postsText.SetText(totalPhotos.ToString());

        string currentFollowers = PlayerPrefs.GetString("CurrentFollowers");
        _followersText.SetText(currentFollowers == "" ? "0" : currentFollowers);
        
        _usernameText.SetText(PlayerPrefs.GetString("Username"));

        SpawnPictureFrames(0, totalPhotos);

        int blankPhotos = (picturesOnEachPage * (((totalPhotos - 1) / picturesOnEachPage) + 1)) - totalPhotos;

        SpawnBlankPictureFrames(blankPhotos);
    }

    private void SpawnPictureFrames(int startIndex, int totalPhotos)
    {
        for (int i = totalPhotos-1; i >= startIndex; i--)
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

    public void OnChangeUsernameButtonClick()
    {
        Debug.Log("Ad Watched");
        _changeUsernamePanel.SetActive(true);
    }

    public void OnChangeUsernameCloseButtonClick()
    {
        _changeUsernamePanel.SetActive(false);
    }

    public void OnChangeUsernameDoneButtonClick()
    {
        string newUsername = _usernameInputField.text;
        
        if (!String.IsNullOrWhiteSpace(newUsername))
        {
            PlayerPrefs.SetString("Username", newUsername);
        }

        _usernameText.SetText(PlayerPrefs.GetString("Username"));
        
        _usernameInputField.text = "";
        _changeUsernamePanel.SetActive(false);
    }
}
