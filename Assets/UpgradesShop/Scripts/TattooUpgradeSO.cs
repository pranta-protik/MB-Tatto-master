using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/TattooUpgradeData")]
public class TattooUpgradeSO : MultiUpgradeDataSO
{
    [SerializeField] private Sprite tattooSprite;
    
    protected override void Awake()
    {
        base.Awake();

        upgradeType = UpgradeType.TattooDesign;
    }

    public Sprite GetTattoo()
    {
        return tattooSprite;
    }
}