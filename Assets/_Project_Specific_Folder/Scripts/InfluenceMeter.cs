using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfluenceMeter : MonoBehaviour
{
    private TextMeshProUGUI _followersText;

    private void Start()
    {
        _followersText = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _followersText.SetText(PlayerPrefs.GetInt("LastFollowers", 0).ToString());
    }
}
