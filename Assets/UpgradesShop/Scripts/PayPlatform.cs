using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PayPlatform : MonoBehaviour
{
    #region Params
    [SerializeField] private float timeToPay = 0.4f;
    [SerializeField] private float depositTravelTime = 0.3f;
    [SerializeField] private Transform depositTarget;
    [SerializeField] private TextMeshPro cashText;
    [SerializeField] private int paymentsAmount = 100;

    private UpgradeDataSO upgradeData;
    private float elapsedTime;
    private int currencyAmount;
    private bool isPaymentOngoing = false;
    private int currencyInTransit;

    private const string PlayerTag = "Player";
    #endregion

    #region Init&Mono
    private void Start()
    {
        currencyInTransit = 0;
        currencyAmount = StorageManager.GetTotalScore();
    }

    private void OnDestroy()
    {
        upgradeData.UpgradesMaxedAction -= OnUpgradesMaxed;
        upgradeData.PaymentSuccessfulAction -= OnPaymentSuccessful;
    }

    public void Init(UpgradeDataSO data)
    {
        upgradeData = data;

        if(!upgradeData.HasPurchasesAvailable())
        {
            gameObject.SetActive(false);
            return;
        }

        SetCashText();

        upgradeData.UpgradesMaxedAction += OnUpgradesMaxed;
        upgradeData.PaymentSuccessfulAction += OnPaymentSuccessful;
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

    private void OnPaymentSuccessful()
    {
        isPaymentOngoing = false;
    }
    #endregion

    #region Logic
    private bool DoPayment()
    {
        int toDeposit;

        if(!upgradeData.CanDeposit(paymentsAmount, currencyInTransit))
        {
            toDeposit = upgradeData.LastDepositAmount(currencyInTransit);

            if(toDeposit <= 0)
            {
                return false;
            }
        }
        else
        {
            toDeposit = paymentsAmount;
        }

        currencyAmount = StorageManager.GetTotalScore();

        currencyAmount = 1000000;

        if(toDeposit > currencyAmount)
        {
            return false;
        }

        Transform cashStack = CurrencyStacksPool.Instance.Pull();
        cashStack.transform.position = transform.position;
        currencyInTransit += toDeposit;
        cashStack.DOMove(depositTarget.position, depositTravelTime).OnComplete(() =>
        {
            currencyInTransit -= toDeposit;
            StorageManager.SetTotalScore(currencyAmount - toDeposit);
            upgradeData.Deposit(toDeposit);
            SetCashText();
            CurrencyStacksPool.Instance.Push(cashStack);
        });

        return true;
    }

    private void SetCashText()
    {
        cashText.SetText((upgradeData.GetNextPurchasePrice() - upgradeData.CurrencyDeposited).ToString());
    }
    #endregion
}