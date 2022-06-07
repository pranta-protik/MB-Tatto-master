using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AvailabilityRate : MonoBehaviour
{
    public TMP_Text Parcentage;
    private void OnEnable()
    {
        int i = Random.Range(0, 6);
        
        if(i==0)
        {
            Parcentage.text = "20";
        }
        if (i == 1)
        {
            Parcentage.text = "25";
        }
        if (i == 2)
        {
            Parcentage.text = "30";
        }
        if (i == 3)
        {
            Parcentage.text = "35";
        }
        if (i == 4)
        {
            Parcentage.text = "40";
        }
        if (i == 5)
        {
            Parcentage.text = "45";
        }
    }
}
