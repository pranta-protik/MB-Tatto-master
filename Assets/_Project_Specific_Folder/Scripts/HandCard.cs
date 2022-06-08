using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour
{
    public int handId;
    public GameObject lockImage;
    public GameObject handImage;
    public GameObject selectionImage;
    public int unlockStatus;

    private void Start()
    {
        lockImage = transform.GetChild(0).gameObject;
        handImage = transform.GetChild(1).gameObject;
        selectionImage = transform.GetChild(2).gameObject;
        
        if (handId == 0)
        {
            unlockStatus = 1;
        }
        
        if (unlockStatus == 0)
        {
            lockImage.SetActive(true);
            handImage.SetActive(false);
        }
        else
        {
            lockImage.SetActive(false);
            handImage.SetActive(true);
        }
    }
}
