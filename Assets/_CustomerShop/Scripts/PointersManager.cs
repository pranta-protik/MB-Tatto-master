using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class PointersManager : Singleton<PointersManager>
{
    [SerializeField] private List<GameObject> pointers;
    [SerializeField] private GameObject tattooSeat;
    [SerializeField] private int tattooSeatPointerIndex;
    private int _currentPointerIndex;

    public override void Start()
    {
        base.Start();
        
        foreach (GameObject pointer in pointers)
        {
            pointer.SetActive(false);
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0 ) == 1)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_UPGRADE_MODEL_ACTIVATION_STATUS, 0) == 0)
            {
                UpgradesManager.Instance.ActivateSelectedStations();
                PlayerPrefs.SetInt(PlayerPrefsKey.TUTORIAL_UPGRADE_MODEL_ACTIVATION_STATUS, 1);
            }
            return;
        }
        
        _currentPointerIndex = PlayerPrefs.GetInt(PlayerPrefsKey.CURRENT_POINTER_INDEX, 0);
        pointers[_currentPointerIndex].SetActive(true);

        if (_currentPointerIndex < tattooSeatPointerIndex)
        {
            tattooSeat.SetActive(false);
        }
    }

    public void EnableNextPointer()
    {
        pointers[_currentPointerIndex].GetComponent<Pointer>().DestroyPointer();

        if (_currentPointerIndex >= pointers.Count-1)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 1);
            return;
        }
        
        _currentPointerIndex++;
        PlayerPrefs.SetInt(PlayerPrefsKey.CURRENT_POINTER_INDEX, _currentPointerIndex);
        pointers[_currentPointerIndex].SetActive(true);

        if (_currentPointerIndex == tattooSeatPointerIndex)
        {
            tattooSeat.SetActive(true);
        }
    }
}
