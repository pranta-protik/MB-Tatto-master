using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
public class CashGenerator :Singleton<CashGenerator>
{
    public List<Transform> CashTransform = new List<Transform>();

    public GameObject CashPrefab;
    public BoxCollider b;
  public void GenerateStack()
    {
        for(int i = 0; i < CashTransform.Count; i++)
        {
            Instantiate(CashPrefab,RandomPointInBounds(b.bounds), Quaternion.Euler(new Vector3(-90, 0, 0) )) ;
        }
    }
    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(3.5f , 3.66f),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
