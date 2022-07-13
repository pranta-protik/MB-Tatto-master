using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Seat : MonoBehaviour
{
    public Reception reception;
    public Transform Exit;
    public Transform SittingPos;
    public GameObject Customer;
    public GameObject CustomerRef;

    public float TargetTime = 0;
    [SerializeField]GameObject _cloneObj;

    public int Timer;

    bool _canTakeCustomer;
    void Start()
    {

        CustomerRef = Instantiate(Customer, SittingPos.transform.position, Quaternion.identity);
        

        int roll = Random.Range(1, 4); // 1, 2 or 3
      
        if (roll == 2) Timer = 3;
        if (roll == 3) Timer = 6;
        if (roll == 1) Timer = 9;

        _canTakeCustomer = true;
    }

    private void Update()
    {
 



        if(_canTakeCustomer)
        {
            print("_canTakeCustomer");
            TargetTime += Time.deltaTime;

            if (TargetTime >= Timer)
            {


                _canTakeCustomer = false;
                TargetTime = 0;
                if (reception.CurrentPassenger != null)
                {
                 
                    reception.CurrentPassenger.transform.DOMove(SittingPos.position, 3).OnComplete(() =>
                    {
                      
                        CustomerRef = reception.CurrentPassenger;
                       
                        reception.CurrentPassenger = null;
                        _canTakeCustomer = true;
                        #region Random Number
                        int roll = Random.Range(1, 4); // 1, 2 or 3

                        if (roll == 2) Timer = 3;
                        if (roll == 3) Timer = 6;
                        if (roll == 1) Timer = 9;
                        #endregion
                    });
                }
                CustomerRef.transform.DOMove(Exit.transform.position, 2).OnComplete(() =>
                {
                   
                      CustomerRef = null;
                });
            }
        }


    }
}
