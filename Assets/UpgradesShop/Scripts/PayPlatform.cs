using System;
using DG.Tweening;
using UnityEngine;

public class PayPlatform : MonoBehaviour
{
    #region Params
    [SerializeField] private UpgradeDataSO upgradeData;
    [SerializeField] private float timeToPay = 0.4f;
    [SerializeField] private Transform depositTarget;
    [SerializeField] private float depositTravelTime = 0.3f;

    private float elapsedTime;
    private int currencyAmount;
    private bool isPaymentOngoing = false;

    private const string PlayerTag = "Player";
    private const int paymentsAmount = 100;
    #endregion

    #region Init&Mono
    private void Awake()
    {
        upgradeData.UpgradesMaxedAction += OnUpgradesMaxed;
    }

    private void Start()
    {
        currencyAmount = StorageManager.GetTotalScore();
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

        isPaymentOngoing = true;
        elapsedTime = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isPaymentOngoing || other.tag != PlayerTag)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= timeToPay)
        {
            elapsedTime -= timeToPay;
            isPaymentOngoing = DoPayment();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != PlayerTag)
        {
            return;
        }

        isPaymentOngoing = false;
    }

    private void OnUpgradesMaxed()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Logic
    private bool DoPayment()
    {
        currencyAmount = StorageManager.GetTotalScore();

        if(paymentsAmount > currencyAmount)
        {
            return false;
        }

        Transform cashStack = CurrencyStacksPool.Instance.Pull();
        cashStack.DOMove(depositTarget.position, depositTravelTime).OnComplete(() =>
        {
            StorageManager.SetTotalScore(currencyAmount - paymentsAmount);
            upgradeData.Deposit(paymentsAmount);
            CurrencyStacksPool.Instance.Push(cashStack);
        });

        return true;
    }
    #endregion
}