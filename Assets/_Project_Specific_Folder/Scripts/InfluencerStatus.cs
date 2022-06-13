using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluencerStatus : MonoBehaviour
{
    public int influencerId;

    private void Start()
    {
        if (PlayerPrefs.GetInt("InfluencerStatus" + influencerId, 0) == 1)
        {
            transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
