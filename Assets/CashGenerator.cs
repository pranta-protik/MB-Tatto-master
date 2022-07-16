using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
public class CashGenerator :Singleton<CashGenerator>
{
    public List<Transform> CashTransform = new List<Transform>();

    public GameObject CashPrefab;

  public void GenerateStack()
    {
        for(int i = 0; i < CashTransform.Count; i++)
        {
            Instantiate(CashPrefab, CashTransform[i].position, Quaternion.Euler(new Vector3(-90, 0, 0) )) ;
        }
    }

}
