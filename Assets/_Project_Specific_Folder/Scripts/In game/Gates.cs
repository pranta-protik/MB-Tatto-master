using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public enum EGateType
{
    Expensive ,
    Cheap
}
public class Gates : MonoBehaviour
{
    [FormerlySerializedAs("IsSpecial")] public bool isSpecial;
    [FormerlySerializedAs("gateGateType")] [FormerlySerializedAs("GateType")] public EGateType gateType;
    [FormerlySerializedAs("Cost")] public int gateCost;
    [FormerlySerializedAs("gateUICostText")] [FormerlySerializedAs("UICostText")] public string gateUICostString;
    [FormerlySerializedAs("CostText")] public TMP_Text gateCostText;
    [FormerlySerializedAs("id")] public int gateId;

    void Start()
    {
        if (gateType == EGateType.Expensive)
        {
            gateCostText.text = "+ $" + gateUICostString;
        }

        else
        {
            gateCostText.text = "- $" + gateUICostString;
        }
    }
}
