using System;
using UnityEngine;

public class PayPlatform : MonoBehaviour
{
    #region Params
    [SerializeField] private UpgradeDataSO upgradeData;
    [SerializeField] private float timeToPay;
    [SerializeField] private CurrencyStacksPool currencyStacksPool;

    private float elapsedTime;

    private const string PlayerTag = "Player";
    private const int paymentsAmount = 100;
    #endregion

    #region Init&Mono
    private void Awake()
    {
        upgradeData.UpgradesMaxedAction += OnUpgradesMaxed;
    }

    private void OnDestroy()
    {
        upgradeData.UpgradesMaxedAction -= OnUpgradesMaxed;
    }
    #endregion

    #region Handlers
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != PlayerTag)
        {
            return;
        }

        elapsedTime = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag != PlayerTag)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= timeToPay)
        {
            elapsedTime -= timeToPay;
            DoPayment();
        }
    }

    private void OnUpgradesMaxed()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Logic
    private void DoPayment()
    {
        upgradeData.Deposit(paymentsAmount);
        //TODO: Animation of money from character to station
    }
    #endregion
}

public class CurrencyStacksPool : MonoBehaviour
{
}