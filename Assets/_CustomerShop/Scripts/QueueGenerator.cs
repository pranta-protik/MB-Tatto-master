using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QueueGenerator : MonoBehaviour
{

    [SerializeField] private Receptionist receptionist;
    [FormerlySerializedAs("Points")] public List<Transform> queuePositions;
    [FormerlySerializedAs("CustomerPrefab")] public GameObject customerPrefab;
    public List<GameObject> customersList;
    
    private GameObject _customer;
    
    public Action<GameObject> CustomerAssignedAction;
    
    void Awake()
    {
        receptionist.SendCustomerAction += OnCustomerRequested;
        
        for (int i = 0; i < queuePositions.Count; i++) {
          _customer =  Instantiate(customerPrefab, queuePositions[i].transform.position, Quaternion.identity);
          customersList.Add(_customer);
        }
    }

    private void OnCustomerRequested()
    {
        CustomerAssignedAction?.Invoke(customersList[0]);
        customersList.RemoveAt(0);
    }

    private void OnDestroy()
    {
        receptionist.SendCustomerAction -= OnCustomerRequested;
    }

    public void Generate(int a)
    {
        for (int i = a; i < queuePositions.Count; i++)
        {
            _customer = Instantiate(customerPrefab, queuePositions[i].transform.position, Quaternion.identity);
            customersList.Add(_customer);
        }
    }
}
