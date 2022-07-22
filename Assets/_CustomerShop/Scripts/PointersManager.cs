using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.Serialization;

public class PointersManager : Singleton<PointersManager>
{
    [FormerlySerializedAs("pointers")] [SerializeField] private List<GameObject> firstTutorialPointers;
    [SerializeField] private GameObject secondTutorialPointer;
    [SerializeField] private GameObject tattooSeat;
    [SerializeField] private int tattooSeatPointerIndex;
    private int _currentPointerIndex;

    public override void Start()
    {
        base.Start();
        
        foreach (GameObject pointer in firstTutorialPointers)
        {
            pointer.SetActive(false);
        }
        
        secondTutorialPointer.SetActive(false);

        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0 ) == 1)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_UPGRADE_MODEL_ACTIVATION_STATUS, 0) == 0)
            {
                UpgradesManager.Instance.ActivateSelectedStations();
                PlayerPrefs.SetInt(PlayerPrefsKey.TUTORIAL_UPGRADE_MODEL_ACTIVATION_STATUS, 1);
            }

            if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_TWO_STATUS, 0) == 1)
            {
                return;
            }

            secondTutorialPointer.SetActive(true);
            
            return;
        }
        
        _currentPointerIndex = PlayerPrefs.GetInt(PlayerPrefsKey.CURRENT_POINTER_INDEX, 0);
        firstTutorialPointers[_currentPointerIndex].SetActive(true);

        if (_currentPointerIndex < tattooSeatPointerIndex)
        {
            tattooSeat.SetActive(false);
        }
    }

    public void EnableNextPointer()
    {
        firstTutorialPointers[_currentPointerIndex].GetComponent<Pointer>().DestroyPointer();

        if (_currentPointerIndex >= firstTutorialPointers.Count-1)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 1);
            PlayerPrefs.SetInt(PlayerPrefsKey.TIP_JAR_UNLOCK_STATUS, 1);
            return;
        }
        
        _currentPointerIndex++;
        PlayerPrefs.SetInt(PlayerPrefsKey.CURRENT_POINTER_INDEX, _currentPointerIndex);
        firstTutorialPointers[_currentPointerIndex].SetActive(true);

        if (_currentPointerIndex == tattooSeatPointerIndex)
        {
            tattooSeat.SetActive(true);
        }
    }

    public void DisableSecondTutorialPointer()
    {
        secondTutorialPointer.GetComponent<Pointer>().DestroyPointer();
        PlayerPrefs.SetInt(PlayerPrefsKey.TUTORIAL_STEP_TWO_STATUS, 1);
    }
}
