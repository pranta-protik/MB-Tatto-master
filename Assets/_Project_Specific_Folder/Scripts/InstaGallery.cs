using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstaGallery : MonoBehaviour
{
    public GameObject pictureFramePrefab;
    
    private void Start()
    {
        Transform spawnTransform = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        Vector2 anchoredSpawnPosition = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>()
            .anchoredPosition;
        
        int totalPhotos = PlayerPrefs.GetInt("SnapshotsTaken", 0);
        transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(totalPhotos.ToString());

        for (int i = 0; i < totalPhotos; i++)
        {
            GameObject pictureFrameObj = Instantiate(pictureFramePrefab, spawnTransform.position, Quaternion.identity, spawnTransform.parent);

            pictureFrameObj.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(anchoredSpawnPosition.x + ((i % 3) * 216), anchoredSpawnPosition.y - ((i / 3) * 217));
        }
    }
    
}
