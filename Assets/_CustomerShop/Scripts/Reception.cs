using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Reception : MonoBehaviour
{
    public QueueGenerator queueGenerator;
    public Transform StandPos;

    public GameObject CurrentPassenger;
   
    void Start()
    {
      
        queueGenerator.Customers[0].transform.DOMove(StandPos.transform.position, 0);
        CurrentPassenger = queueGenerator.Customers[0].gameObject;
        queueGenerator.Customers.RemoveAt(0);

    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentPassenger == null)
        {

            queueGenerator.Customers[0].transform.DOMove(StandPos.transform.position, 2);
            CurrentPassenger = queueGenerator.Customers[0].gameObject;
            queueGenerator.Customers.RemoveAt(0);
        }
    }
}
