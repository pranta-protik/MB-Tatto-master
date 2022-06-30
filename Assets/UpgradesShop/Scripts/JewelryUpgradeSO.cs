using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/JewelryUpgradeData")]
public class JewelryUpgradeSO : MultiUpgradeDataSO
{
    [SerializeField] private GameObject jewelryPrefab;
    
    protected override void Awake()
    {
        base.Awake();
        
        upgradeType = UpgradeType.Jewelry;
    }

    public GameObject GetJewelry()
    {
        return jewelryPrefab;
    }
}