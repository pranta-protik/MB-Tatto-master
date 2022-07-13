using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/TattooUpgradeData")]
public class TattooUpgradeSO : MultiUpgradeDataSO
{
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