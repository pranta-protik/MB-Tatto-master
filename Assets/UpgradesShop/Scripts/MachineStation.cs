using System.Collections.Generic;
using UnityEngine;

public class MachineStation : UpgradeStation
{
    #region Params
    [SerializeField] private List<GameObject> levels;

    private MachineUpgradeSO machineUpgradeData;
    private GameObject currentLevelObj;
    #endregion

    #region Init&Mono
    protected override void Awake()
    {
        base.Awake();

        machineUpgradeData = upgradeData as MachineUpgradeSO;
        machineUpgradeData.LevelChangedAction += OnLevelChanged;

        ActivateMachineLevel();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        machineUpgradeData.LevelChangedAction -= OnLevelChanged;
    }
    #endregion

    #region Handlers
    private void OnLevelChanged(int newLevel)
    {
        currentLevelObj.SetActive(false);
        currentLevelObj = levels[newLevel - 1];
        currentLevelObj.SetActive(true);
    }
    #endregion

    #region Logic
    private void ActivateMachineLevel()
    {
        for(int i = 0, count = levels.Count; i < count; i++)
        {
            if(i == machineUpgradeData.UnlockedLevel - 1)
            {
                levels[i].SetActive(true);
                currentLevelObj = levels[i];
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
    }
    #endregion
}