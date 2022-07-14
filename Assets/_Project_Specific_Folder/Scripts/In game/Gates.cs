using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public enum EGateType
{
    Expensive ,
    Cheap
}
public class Gates : MonoBehaviour
{
    [FormerlySerializedAs("gateGateType")] [FormerlySerializedAs("GateType")] public EGateType gateType;
    [FormerlySerializedAs("Cost")] public int gateCost;
    [FormerlySerializedAs("gateUICostText")] [FormerlySerializedAs("UICostText")] public string gateUICostString;
    [FormerlySerializedAs("IsSpecial")] public bool isSpecial;
    public bool isLast;
    [FormerlySerializedAs("gateId")] [FormerlySerializedAs("id")] public int gateLevel;

    private TMP_Text _gateCostText;

    void Start()
    {
        _gateCostText = transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
        
        if (gateType == EGateType.Expensive)
        {
            _gateCostText.text = "+ $" + gateUICostString;
        }
        else if(gateType == EGateType.Cheap)
        {
            _gateCostText.text = "- $" + gateUICostString;
        }
    }
}
