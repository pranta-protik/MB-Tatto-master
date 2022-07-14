using System.Collections.Generic;
using HomaGames.HomaConsole.Performance.Utils;
using UnityEngine;

public class CurrencyStacksPool : Performance_Singleton<CurrencyStacksPool>
{
    [SerializeField] private GameObject currencyStackPrefab;
    [SerializeField] private int initialPoolSize;
    
    private Queue<Transform> currencyStacksQueue;
    private Transform auxTransform;
    private GameObject auxObj;

    private void Start()
    {
        currencyStacksQueue = new Queue<Transform>();

        for(int i = 0; i < initialPoolSize; i++)
        {
            auxObj = Instantiate(currencyStackPrefab, transform);
            auxObj.SetActive(false);
            currencyStacksQueue.Enqueue(auxObj.transform);
        }
    }

    public void Push(Transform stack)
    {
        stack.gameObject.SetActive(false);
        stack.localPosition = Vector3.zero;
        currencyStacksQueue.Enqueue(stack);
    }
    
    public Transform Pull()
    {
        auxTransform = currencyStacksQueue.Dequeue();

        if(auxTransform == null)
        {
            auxObj = Instantiate(currencyStackPrefab, transform);
            auxTransform = auxObj.transform;
        }

        auxTransform.gameObject.SetActive(true);
        
        return auxTransform;
    }
    
    
}