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
                queueGenerator.Customers[0].transform.GetChild(0).GetComponent<CharacterUnlock>().anim.SetTrigger("Walk");
                queueGenerator.Customers[0].transform.DOMove(StandPos.transform.position, 3).SetEase(Ease.Linear).OnComplete(() =>
                {
                    queueGenerator.Customers[0].transform.GetChild(0).GetComponent<CharacterUnlock>().anim.SetTrigger("idle");
                    CurrentPassenger = queueGenerator.Customers[0].gameObject;
                    queueGenerator.Customers.RemoveAt(0);


                });
                for (int i = 0; i < queueGenerator.Customers.Count ; i++)
                {
                    queueGenerator.Customers[i].transform.DOMove(queueGenerator.Points[i].transform.position, .5f).SetEase(Ease.InSine);
                }


                if(queueGenerator.Customers.Count < 5)
                {
                    queueGenerator.Generate(5);
                }


                Played = true;
            }

        }
    }

}
