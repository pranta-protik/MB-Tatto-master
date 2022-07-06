using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/TattooUpgradeData")]
public class TattooUpgradeSO : MultiUpgradeDataSO
{
    [SerializeField] private Sprite tattooSprite;
    
    public override void Init()
    {
        base.Init();

        upgradeType = UpgradeType.TattooDesign;
    }

    public Sprite GetTattoo()
    {
        return tattooSprite;
    }
}