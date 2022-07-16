using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Reception : MonoBehaviour
{
    public QueueGenerator queueGenerator;
    public Transform StandPos;

    public GameObject CurrentPassenger;

    public bool Played;

    void Start()
    {
       
        queueGenerator.Customers[0].transform.DOMove(StandPos.transform.position, 0);
        CurrentPassenger = queueGenerator.Customers[0].gameObject;
        queueGenerator.Customers.RemoveAt(0);

    }


    private void Update()
    {
        if (CurrentPassenger == null)
        {
            if (!Played)
            {
                queueGenerator.Customers[0].transform.DOMove(StandPos.transform.position, 2).OnComplete(() =>
                {

                    CurrentPassenger = queueGenerator.Customers[0].gameObject;
                    queueGenerator.Customers.RemoveAt(0);


                });
                for (int i = 0; i < queueGenerator.Customers.Count ; i++)
                {
                    queueGenerator.Customers[i].transform.DOMove(queueGenerator.Points[i].transform.position, .1f);
                }

                Played = true;
            }

        }
    }

}
