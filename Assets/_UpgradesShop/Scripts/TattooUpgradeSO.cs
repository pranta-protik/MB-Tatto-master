using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/TattooUpgradeData")]
public class TattooUpgradeSO : MultiUpgradeDataSO
{
    public int serialNo;
    [SerializeField] private Sprite tattooSprite;
    
    public override void Init()
    {
        upgradeType = UpgradeType.TattooDesign;
        base.Init();
    }

    public Sprite GetTattoo()
    {
        return tattooSprite;
    }
}