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
    public int Cost;
    public string UICostText;

    public TMP_Text CostText;


    public int id;

    void Start()
    {
        if(GateType ==EType.Expensive)
        CostText.text = "+ $" +UICostText;
        else
        CostText.text = "- $" + UICostText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
