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

    public int Timer;

    [SerializeField]bool _hasCustomer;
    GameObject _customerRef;
    [SerializeField] GameObject _cloneObj;
    void Start()
    {

        RandomNumberGenerator();
        if (GetComponentInParent<Shop>() != null)
        {

            if (!GetComponentInParent<Shop>().IsLocked)
            {
                _customerRef = Instantiate(Customer, SittingPos.transform.position, Quaternion.identity);
                _hasCustomer = true;
            }
        }
        else
        {
            _customerRef = Instantiate(Customer, SittingPos.transform.position, Quaternion.identity);
            _hasCustomer = true;
            CashGenerator.Instance.GenerateStack();
        }
       


       
    }

    private void Update()
    {
        if (!_hasCustomer)
        {
            if (reception.CurrentPassenger != null)
            {
                _customerRef = reception.CurrentPassenger;
                reception.CurrentPassenger.transform.DOMove(SittingPos.position, 2).OnComplete(() =>
              {
               
                  _hasCustomer = true;
                  RandomNumberGenerator();


              }); 
                reception.CurrentPassenger = null; reception.Played = false;
            }

        }




        if (_hasCustomer)
        {
           

            CustomerRef = _customerRef;
            TargetTime += Time.deltaTime;

            if (TargetTime >= Timer)
            {

                _hasCustomer = false;

                TargetTime = 0;
                CustomerRef.transform.DOMove(Exit.position, 2).SetEase(Ease.InSine).OnComplete(() =>
                {
                    CustomerRef.transform.DOMoveX(CustomerRef.transform.position.x - 10,3).OnComplete(() =>
                    {




                    });
                    Destroy(CustomerRef,2);


                });
            }

        }
    }

    private void RandomNumberGenerator()
    {
        int roll = Random.Range(1, 4); // 1, 2 or 3

        if (roll == 2) Timer = 3;
        if (roll == 3) Timer = 6;
        if (roll == 1) Timer = 9;
    }
}
