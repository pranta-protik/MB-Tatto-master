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
    [Header("Seats")]
    public GameObject[] Seats;
    public GameObject UnlockFX;
    public int GenerateAmmount;
    [Header("UI")]
    public TMP_Text CurrentCostText;
    public TMP_Text TotalCostText;
    public GameObject canvas , SeatUnlockCanvas;

    public bool Instant;

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
            CashGenerator.Instance.GenerateStack();
            CheckForUnlockedSeat();

            IsLocked = false;
            LockedMesh.gameObject.SetActive(false);
            UnlockedMesh.gameObject.SetActive(true);
            //UnlockedMesh.transform.GetComponent<MySDK.Scaler>().enabled = false;
            UnlockedMesh.transform.DOScale(new Vector3(1, 1, 1), 0);
            canvas.gameObject.SetActive(false);

            GetComponent<BoxCollider>().enabled = false;


        }


    }

    private void CheckForUnlockedSeat()
    {
        if (PlayerPrefs.GetInt("CurrentSeat" + Id) == 0)
        {
            Seats[0].SetActive(true);
        }
        else if (PlayerPrefs.GetInt("CurrentSeat" + Id) == 01)
        {
            Seats[01].SetActive(true);
        }
        else
        {
            Seats[02].SetActive(true);
        }
    }

    public void InstantUnlock()
    {
            SeatUnlockCanvas.gameObject.SetActive(true);
          
            LockedMesh.gameObject.SetActive(false);
            
            //UnlockedMesh.transform.GetComponent<MySDK.Scaler>().enabled = false;
            UnlockedMesh.transform.DOScale(new Vector3(1, 1, 1), 0);
            canvas.gameObject.SetActive(false);
            PlayerPrefs.SetInt("CurrentCost" + Id, TotalCost); 
            GetComponent<BoxCollider>().enabled = false;


        


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

    public void UnlockSeat1()
    {
        Instantiate(UnlockFX, Seats[0].transform.position, Quaternion.identity);
        UnlockedMesh.gameObject.SetActive(true);
        SeatUnlockCanvas.gameObject.SetActive(false);
        Seats[0].gameObject.SetActive(true);
        PlayerPrefs.SetInt("CurrentSeat" + Id, 0);
    }
    public void UnlockSeat2()
    {
        Instantiate(UnlockFX, Seats[01].transform.position, Quaternion.identity);
        UnlockedMesh.gameObject.SetActive(true);
        SeatUnlockCanvas.gameObject.SetActive(false);
        Seats[01].gameObject.SetActive(true);
        PlayerPrefs.SetInt("CurrentSeat" + Id, 1);
    }
    public void UnlockSeat3()
    {
        Instantiate(UnlockFX, Seats[02].transform.position, Quaternion.identity);
        UnlockedMesh.gameObject.SetActive(true);
        SeatUnlockCanvas.gameObject.SetActive(false);
        Seats[02].gameObject.SetActive(true);
        PlayerPrefs.SetInt("CurrentSeat" + Id, 2);
    }
}
