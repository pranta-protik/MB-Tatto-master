using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class CashGenerator : Singleton<CashGenerator>
{
    // public List<Transform> CashTransform = new List<Transform>();
    [SerializeField] private int multiplier;
    public GameObject CashPrefab;
    public BoxCollider boxCollider;

    public override void Start()
    {
        base.Start();

        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0) == 0)
        {
            return;
        }
        
        GenerateStack();
    }

    private void GenerateStack()
    {
        for (int i = 0; i < PlayerPrefs.GetInt(PlayerPrefsKey.UNLOCKED_TATTOO_SEATS, 0) * multiplier; i++)
        {
            Instantiate(CashPrefab, RandomPointInBounds(boxCollider.bounds), Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }
    public void GenerateSingleStack()
    {
        Instantiate(CashPrefab, RandomPointInBounds(boxCollider.bounds), Quaternion.Euler(new Vector3(-90, 0, 0)));
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
