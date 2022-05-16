using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum EType
{
    Expensive ,
    Cheap
}
public class Gates : MonoBehaviour
{
    public EType GateType;
    public float Cost;
    public TMP_Text CostText;

    public int id;

    void Start()
    {
        CostText.text = Cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
