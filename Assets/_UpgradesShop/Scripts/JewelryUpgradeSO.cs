using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/JewelryUpgradeData")]
public class JewelryUpgradeSO : MultiUpgradeDataSO
{
    public int serialNo;
    [SerializeField] private GameObject jewelryPrefab;
    
    public override void Init()
    {
        upgradeType = UpgradeType.Jewelry;
        base.Init();
    }

    public GameObject GetJewelry()
    {
        return jewelryPrefab;
    }
}