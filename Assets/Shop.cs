using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{

    public string Name;
    public string Id;

    public bool CanConjure;
    public bool IsLocked = true;
    public int TotalCost;
    public int CurrentCost;
    public GameObject UnlockedMesh, LockedMesh;
    public int PerSecond;
    [Header("Money Generation")]

    public int GenerateAmmount;
    [Header("UI")]
    public TMP_Text CurrentCostText;
    public TMP_Text TotalCostText;
    public GameObject canvas;



    private void Start()
    {

        IsLocked = true;
        CurrentCostText.text = PlayerPrefs.GetInt("CurrentCost" + Id).ToString() + "K";
        TotalCostText.text = "/ " + TotalCost.ToString() + "K";
        UnlockCheck01();
  
    }

    private void UnlockCheck01()
    {
        if (PlayerPrefs.GetInt("CurrentCost" + Id) >= TotalCost)
        {
            IsLocked = false;
            LockedMesh.gameObject.SetActive(false);
            UnlockedMesh.gameObject.SetActive(true);
            UnlockedMesh.transform.GetComponent<MySDK.Scaler>().enabled = false;
            UnlockedMesh.transform.DOScale(new Vector3(1, 1, 1), 0);
            canvas.gameObject.SetActive(false);
          
            GetComponent<BoxCollider>().enabled = false;


        }


    }

    public void AddMoney(int Ammount)
    {
        CurrentCost = PlayerPrefs.GetInt("CurrentCost" + Id);
        CurrentCost += Ammount;
        PlayerPrefs.SetInt("CurrentCost" + Id, CurrentCost);
        CurrentCostText.text = CurrentCost.ToString() + "K";
        CheckifUnlocked();
    }

    private void CheckifUnlocked()
    {
        if (PlayerPrefs.GetInt("CurrentCost" + Id) >= TotalCost)
        {
            IsLocked = false;
            LockedMesh.gameObject.SetActive(false);
            UnlockedMesh.gameObject.SetActive(true);
            canvas.gameObject.SetActive(false);
            Invoke("GeneratePanelDelay", 1);
            GetComponent<BoxCollider>().enabled = false;
          


        }
    }

    public void GeneratePanelDelay()
    {
      
    }
}
