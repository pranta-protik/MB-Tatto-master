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
        int i = Random.Range(0, 5);
        if(i==0)
        {
            Parcentage.text = "40%";
        }
        if (i == 01)
        {
            Parcentage.text = "45%";
        }
        if (i == 02)
        {
            Parcentage.text = "50%";
        }
        if (i == 03)
        {
            Parcentage.text = "55%";
        }
        if (i == 04)
        {
            Parcentage.text = "60%";
        }
      
    }
}
