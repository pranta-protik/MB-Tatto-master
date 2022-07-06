using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/JewelryUpgradeData")]
public class JewelryUpgradeSO : MultiUpgradeDataSO
{
    [SerializeField] private GameObject jewelryPrefab;
    
    public override void Init()
    {
        base.Init();
        
        upgradeType = UpgradeType.Jewelry;
    }

    public GameObject GetJewelry()
    {
        return jewelryPrefab;
    }
}