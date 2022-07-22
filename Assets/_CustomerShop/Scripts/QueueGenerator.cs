using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class QueueGenerator : MonoBehaviour
{

    [SerializeField] private Receptionist receptionist;
    [FormerlySerializedAs("Points")] public List<Transform> queuePositions;
    [FormerlySerializedAs("CustomerPrefab")] public GameObject[] customerPrefabs;
    [SerializeField] private List<AnimationClip> customerAnimationClips;
    [SerializeField] private Transform customersParent;
    public List<GameObject> customersList;
    
    private GameObject _customer;
    
    public Action<GameObject> CustomerAssignedAction;
    
    void Awake()
    {
        receptionist.SendCustomerAction += OnCustomerRequested;

        foreach (Transform queuePosition in queuePositions)
        {
            int index = Random.Range(0, customerPrefabs.Length);
            _customer =  Instantiate(customerPrefabs[index], queuePosition.transform.position, queuePosition.transform.rotation, customersParent);
            
            int clipIndex = Random.Range(0, customerAnimationClips.Count);
            _customer.GetComponent<Customer>().SetIdleAnimation(customerAnimationClips[clipIndex]);
            
            customersList.Add(_customer);
        }
        
        customersParent.gameObject.SetActive(false);
    }

    private void OnCustomerRequested()
    {
        if (!customersParent.gameObject.activeSelf)
        {
            customersParent.gameObject.SetActive(true);
        }
        
        CustomerAssignedAction?.Invoke(customersList[0]);
        customersList.RemoveAt(0);
        
        for (int i = 0; i < customersList.Count ; i++)
        {
            customersList[i].transform.DOMove(queuePositions[i].transform.position, .5f).SetEase(Ease.InSine);
        }

        if (customersList.Count<5)
        {
            Generate(5);
        }
    }

    private void OnDestroy()
    {
        receptionist.SendCustomerAction -= OnCustomerRequested;
    }

    private void Generate(int generateFrom)
    {
        for (int i = generateFrom; i < queuePositions.Count; i++)
        {
            int index = Random.Range(0, customerPrefabs.Length);
            _customer =  Instantiate(customerPrefabs[index], queuePositions[i].transform.position, queuePositions[i].transform.rotation, customersParent);
            
            int clipIndex = Random.Range(0, customerAnimationClips.Count);
            _customer.GetComponent<Customer>().SetIdleAnimation(customerAnimationClips[clipIndex]);
            
            customersList.Add(_customer);
        }
    }
}
