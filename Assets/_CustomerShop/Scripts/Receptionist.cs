using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Receptionist : MonoBehaviour
{
    [SerializeField] private ReceptionDesk receptionDesk;
    [SerializeField] private Transform waitingPosition;
    [SerializeField] private QueueGenerator queueGenerator;
    [SerializeField] private List<TattooSeat> customerRequestQueue;
    public GameObject WaitingCustomer { get; private set; }
    public bool IsCustomerReady { get; set; }
    public Action SendCustomerAction;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Start()
    {
        queueGenerator.CustomerAssignedAction += OnCustomerCalled;
        
        if (receptionDesk.IsUnlocked && WaitingCustomer == null)
        {
            SendCustomerAction?.Invoke();
        }
    }

    private void OnDestroy()
    {
        queueGenerator.CustomerAssignedAction -= OnCustomerCalled;
    }

    private void OnCustomerCalled(GameObject customerObj)
    {
        WaitingCustomer = customerObj;

        WaitingCustomer.transform.LookAt(waitingPosition);
        WaitingCustomer.transform.GetChild(0).LookAt(waitingPosition);
        
        WaitingCustomer.transform.GetChild(0).GetComponent<Animator>().SetBool(IsWalking, true);
        WaitingCustomer.transform.DOMove(waitingPosition.position, 3).SetEase(Ease.Linear).OnComplete(() =>
        {
            WaitingCustomer.transform.GetChild(0).GetComponent<Animator>().SetBool(IsWalking, false);
            IsCustomerReady = true;
        });
    }

    public void AddRequestToQueue(TattooSeat tattooSeat)
    {
        customerRequestQueue.Add(tattooSeat);
    }

    private void Update()
    {
        if (WaitingCustomer != null && customerRequestQueue.Count > 0 && IsCustomerReady)
        {
            SendCustomerToEmptySeat();
        }
    }

    private void SendCustomerToEmptySeat()
    {
        TattooSeat tattooSeat = customerRequestQueue[0];
        customerRequestQueue.RemoveAt(0);
        tattooSeat.TattooCustomer = WaitingCustomer;
        tattooSeat.ApplyTattoo();
        
        WaitingCustomer = null;
        IsCustomerReady = false;
        SendCustomerAction?.Invoke();
    }
}