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
    [SerializeField] GameObject _customerRef;
    [SerializeField] GameObject _cloneObj;
    void Start()
    {

        RandomNumberGenerator();
        if (GetComponentInParent<Shop>() != null)
        {

            if (!GetComponentInParent<Shop>().IsLocked)
            {
                _customerRef = Instantiate(Customer, SittingPos.transform.position, Quaternion.identity);
                _customerRef.GetComponentInChildren<CharacterUnlock>().transform.DOLocalRotate(new Vector3(0, -180, 0), 0f);
                _customerRef.transform.DOLocalMoveX(_customerRef.transform.localPosition.x - .8f, 0);
                _customerRef.GetComponentInChildren<CharacterUnlock>().anim.Play("Sitting");

                _hasCustomer = true;
            }
        }
        else
        {
            _customerRef = Instantiate(Customer, SittingPos.transform.position, Quaternion.identity);
            _customerRef.GetComponentInChildren<CharacterUnlock>().transform.DOLocalRotate(new Vector3(0, -180, 0), 0f);
            _customerRef.transform.DOLocalMoveX(_customerRef.transform.localPosition.x - .8f, 0);
            _customerRef.GetComponentInChildren<CharacterUnlock>().anim.Play("Sitting");

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
                reception.CurrentPassenger.transform.GetChild(0).GetComponent<CharacterUnlock>().anim.SetTrigger("Walk");
                reception.CurrentPassenger.transform.GetChild(0).LookAt(transform.position);
                if ((GetComponentInParent<Shop>() == null))
                {
                    reception.CurrentPassenger.transform.DOMove(SittingPos.position, 2).OnComplete(() =>
                    {

                        _hasCustomer = true;
                        RandomNumberGenerator();
                        _customerRef.transform.DOLocalMoveX(_customerRef.transform.localPosition.x - .8f, 0);

                        _customerRef.transform.GetChild(0).DOLocalRotate(new Vector3(0, 180, 0), 0);
                    });
                }


                else
                {
                    if (GetComponentInParent<Shop>().Id == "1")
                    {
                        reception.CurrentPassenger.transform.DOMove(SittingPos.position, 2).OnComplete(() =>
                      {

                          _hasCustomer = true;
                          RandomNumberGenerator();
                          _customerRef.transform.DOLocalMoveX(_customerRef.transform.localPosition.x - .8f, 0);

                          _customerRef.transform.GetChild(0).DOLocalRotate(new Vector3(0, 180, 0), 0);
                      });
                    }
                    else
                    {
                        reception.CurrentPassenger.transform.DOMove(SittingPos.position, 3).OnComplete(() =>
                        {

                            _hasCustomer = true;
                            RandomNumberGenerator();
                            _customerRef.transform.DOLocalMoveX(_customerRef.transform.localPosition.x - .8f, 0);

                            _customerRef.transform.GetChild(0).DOLocalRotate(new Vector3(0, 180, 0), 0);
                        });
                    }
                }
                reception.CurrentPassenger = null; reception.Played = false;
            }

        }




        if (_hasCustomer)
        {
           if(TargetTime > 0)
            {
               // _customerRef.GetComponentInChildren<CharacterUnlock>().transform.DOLocalRotate(new Vector3(0, -180, 0), .2f);
                _customerRef.transform.GetChild(0).GetComponent<CharacterUnlock>().anim.Play("Sitting");
                transform.GetComponentInChildren<CharacterUnlock>().anim.SetBool("CanWrite", true);
            } 
   


            CustomerRef = _customerRef;

            TargetTime += Time.deltaTime;

            if (TargetTime >= Timer)
            {

                _hasCustomer = false;
               
                TargetTime = 0;
                CustomerRef.transform.GetChild(0).LookAt(Exit);
                transform.GetComponentInChildren<CharacterUnlock>().anim.SetBool("CanWrite",false);
                CustomerRef.transform.GetChild(0).GetComponent<CharacterUnlock>().anim.SetTrigger("Walk");
                CustomerRef.transform.DOMove(Exit.position, 2).SetEase(Ease.InSine).OnComplete(() =>
                {
                    CustomerRef.transform.GetChild(0).DOLocalRotate(new Vector3(0, -90, 0), 0);
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
