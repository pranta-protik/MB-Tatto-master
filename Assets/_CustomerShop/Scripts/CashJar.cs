using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CashJar : MonoBehaviour
{
    public GameObject Full, Empty;
    public TMP_Text CashAmmountText;
    public int CashAmmount { get; private set; }
    private void Start()
    {
        int roll = Random.Range(1, 4);

        if (roll == 2) CashAmmount = 15000;
        if (roll == 3) CashAmmount = 30000;
        if (roll == 1) CashAmmount = 50000;

        CashAmmountText.text = CashAmmount.ToString();
    }
    public void Jar()
    {
       
       Full.GetComponent<MeshRenderer>().enabled = false;
       Empty.gameObject.SetActive(true);
       CashAmmountText.transform.parent.gameObject.SetActive(false);
       StorageManager.SetTotalScore(StorageManager.GetTotalScore() + CashAmmount);

    }
}



