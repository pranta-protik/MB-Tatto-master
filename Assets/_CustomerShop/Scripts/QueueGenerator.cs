using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueGenerator : MonoBehaviour
{
    public List<Transform> Points;

    public GameObject CustomerPrefab;

    GameObject _customerPrefab;
    public List<GameObject> Customers;
    void Awake()
    {
        for (int i = 0; i < Points.Count; i++) {
          _customerPrefab =  Instantiate(CustomerPrefab, Points[i].transform.position, Quaternion.identity);
          Customers.Add(_customerPrefab);
        }
    }
    public void Generate(int a)
    {
        for (int i = a; i < Points.Count; i++)
        {
            _customerPrefab = Instantiate(CustomerPrefab, Points[i].transform.position, Quaternion.identity);
            Customers.Add(_customerPrefab);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
