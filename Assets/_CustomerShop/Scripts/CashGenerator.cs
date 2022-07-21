using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class CashGenerator : Singleton<CashGenerator>
{
    public List<Transform> CashTransform = new List<Transform>();
    public GameObject CashPrefab;
    public BoxCollider boxCollider;

    public void GenerateStack()
    {
        for (int i = 0; i < CashTransform.Count; i++)
        {
            Instantiate(CashPrefab, RandomPointInBounds(boxCollider.bounds), Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }

    private Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(3.5f, 3.66f),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
