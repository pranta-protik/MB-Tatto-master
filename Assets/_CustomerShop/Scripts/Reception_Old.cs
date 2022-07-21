using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Reception_Old : MonoBehaviour
{
    public QueueGenerator queueGenerator;
    public Transform StandPos;

    public GameObject CurrentPassenger;

    public bool Played;

    void Start()
    {
       
        queueGenerator.customersList[0].transform.DOMove(StandPos.transform.position, 0);
        CurrentPassenger = queueGenerator.customersList[0].gameObject;
        queueGenerator.customersList.RemoveAt(0);
    }
    
    private void Update()
    {
        if (CurrentPassenger == null)
        {
            if (!Played)
            {
                queueGenerator.customersList[0].transform.GetChild(0).GetComponent<CharacterUnlock>().anim.Play("Walking");
                queueGenerator.customersList[0].transform.DOMove(StandPos.transform.position, 3).SetEase(Ease.Linear).OnComplete(() =>
                {
                    queueGenerator.customersList[0].transform.GetChild(0).GetComponent<CharacterUnlock>().anim.Play("idle 0");
                    CurrentPassenger = queueGenerator.customersList[0].gameObject;
                    queueGenerator.customersList.RemoveAt(0);


                });
                for (int i = 0; i < queueGenerator.customersList.Count ; i++)
                {
                    queueGenerator.customersList[i].transform.DOMove(queueGenerator.queuePositions[i].transform.position, .5f).SetEase(Ease.InSine);
                }


                if(queueGenerator.customersList.Count < 5)
                {
                    // queueGenerator.Generate(5);
                }


                Played = true;
            }

        }
    }

}
