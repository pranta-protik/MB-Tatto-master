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
        //int j = Random.Range(0, 3);

        //if (j == 0) _customerPrefab.GetComponentInChildren<CharacterUnlock>().anim.Play("idle 0");
        //if (j == 1) _customerPrefab.GetComponentInChildren<CharacterUnlock>().anim.Play("idle 1");
        //if (j == 2) _customerPrefab.GetComponentInChildren<CharacterUnlock>().anim.Play("idle");
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
