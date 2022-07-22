using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TattooSeatUnlockPlatform : MonoBehaviour
{
    [SerializeField] private float timeToPay = 0.1f;
    [SerializeField] private float depositTravelTime = 0.05f;
    [SerializeField] private Transform depositTarget;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private int paymentsAmount = 100;
    [SerializeField] private Image progressBar;
    [SerializeField] private float second;
    
    private TattooSeat _tattooSeat;
    private int _currencyInTransit;
    private int _currencyAmount;
    private float _time;
    private bool _isProgressBarFilling;
    private bool _isPaymentOngoing;
    private float _elapsedTime;
    
    private const string PlayerTag = "Player";
    
    private void Start()
    {
        _currencyInTransit = 0;
        _currencyAmount = StorageManager.GetTotalScore();
    }

    private void OnDestroy()
    {
        _tattooSeat.PaymentSuccessfulAction -= OnPaymentSuccessful;
    }

    public void Init(TattooSeat tattooSeat)
    {
        _tattooSeat = tattooSeat;

        SetCashText();
    
        if (_tattooSeat.IsUnlocked)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        
        _tattooSeat.PaymentSuccessfulAction += OnPaymentSuccessful;
    }

    private void OnPaymentSuccessful()
    {
        _isPaymentOngoing = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        _isProgressBarFilling = true;
        _isPaymentOngoing = false;
        _elapsedTime = 0f;
        _time = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        if (_isProgressBarFilling)
        {
            if (_time < second)
            {
                _time += Time.deltaTime;
                progressBar.fillAmount = _time / second;
            }
            else
            {
                _isPaymentOngoing = true;
                _isProgressBarFilling = false;
            }
        }

        if (!_isProgressBarFilling && _isPaymentOngoing)
        {
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime >= timeToPay)
            {
                _elapsedTime -= timeToPay;
                _isPaymentOngoing = DoPayment();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        _isProgressBarFilling = false;
        progressBar.fillAmount = 0;
        
        _isPaymentOngoing = false;
    }

    private bool DoPayment()
    {
        int toDeposit;
    
        if (!_tattooSeat.CanDeposit(paymentsAmount, _currencyInTransit))
        {
            toDeposit = _tattooSeat.LastDepositAmount(_currencyInTransit);
    
            if (toDeposit <= 0)
            {
                return false;
            }
        }
        else
        {
            toDeposit = paymentsAmount;
        }
    
        _currencyAmount = StorageManager.GetTotalScore();
    
        if (toDeposit > _currencyAmount)
        {
            return false;
        }
    
        Transform cashStack = CurrencyStacksPool.Instance.Pull();
        cashStack.transform.position = transform.position;
        _currencyInTransit += toDeposit;
    
        cashStack.DOMove(depositTarget.position, depositTravelTime).OnComplete(() =>
        {
            _currencyInTransit -= toDeposit;
            StorageManager.SetTotalScore(_currencyAmount - toDeposit);
            _tattooSeat.Deposit(toDeposit);
            SetCashText();
            CurrencyStacksPool.Instance.Push(cashStack);
        });
    
        return true;
    }
    
    private void SetCashText()
    {
        cashText.SetText($"${CurrencySystem.GetConvertedCurrencyString(_tattooSeat.UnlockPrice - _tattooSeat.CurrencyDeposited)}");
    }
}
