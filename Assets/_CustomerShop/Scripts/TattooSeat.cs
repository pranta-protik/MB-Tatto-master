using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TattooSeat : MonoBehaviour
{
    [SerializeField] private int tattooSeatId;
    [SerializeField] private Transform tattooSeatParent;
    [SerializeField] private GameObject decorativeObject;
    [SerializeField] private GameObject tattooArtist;
    [SerializeField] private TattooSeatSelectionPanel tattooSeatSelectionPanel;
    [SerializeField] private int unlockPrice;
    [SerializeField] private TattooSeatUnlockPlatform tattooSeatUnlockPlatform;
    // [SerializeField] private Pointer pointer;
    
    public Action PaymentSuccessfulAction;
    
    public int UnlockPrice => unlockPrice;
    public bool IsUnlocked => _isUnlocked;
    
    public int CurrencyDeposited
    {
        get => _currencyDeposited;
        private set
        {
            _currencyDeposited = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.TATTOO_SEAT_CURRENCY_DEPOSITED + tattooSeatId, _currencyDeposited);
        }
    }

    private bool _isUnlocked;
    private int _currencyDeposited;
    
    private void Awake()
    {
        _currencyDeposited = PlayerPrefs.GetInt(PlayerPrefsKey.TATTOO_SEAT_CURRENCY_DEPOSITED + tattooSeatId, 0);
        _isUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey.TATTOO_SEAT_UNLOCK_STATUS + tattooSeatId, 0) == 1;
        
        if (!_isUnlocked)
        {
            return;
        }
        
        int id = PlayerPrefs.GetInt(PlayerPrefsKey.TATTOO_SELECTED_SEAT_ID + tattooSeatId, 0);
        Instantiate(TattooSeatsManager.Instance.GetTattooSeat(id), tattooSeatParent);
        
        decorativeObject.SetActive(true);
        tattooArtist.SetActive(true);
        // pointer.DestroyPointer();
    }

    private void Start()
    {
        tattooSeatUnlockPlatform.Init(this);
    }

    public bool CanDeposit(int amount, int amountInTransit)
    {
        if(unlockPrice - _currencyDeposited - amountInTransit > amount)
        {
            return true;
        }

        return false;
    }
    
    public int LastDepositAmount(int amountInTransit)
    {
        return unlockPrice - _currencyDeposited - amountInTransit;
    }
    
    public void Deposit(int amount)
    {
        CurrencyDeposited += amount;
        
        if(_currencyDeposited >= unlockPrice)
        {
            if(_currencyDeposited > unlockPrice)
            {
                Debug.LogError("[UPGRADES] To much money deposited for reception desk");
            }

            Unlock();
            CurrencyDeposited = 0;
        }
    }

    private void Unlock()
    {
        tattooSeatSelectionPanel.ShowPanel();
        tattooSeatSelectionPanel.TattooSeatUnlockAction += OnTattooSeatUnlocked;
        
        PaymentSuccessfulAction?.Invoke();
        
        decorativeObject.SetActive(true);
        tattooArtist.SetActive(true);
        // pointer.DestroyPointer();
        _isUnlocked = true;
        PlayerPrefs.SetInt(PlayerPrefsKey.TATTOO_SEAT_UNLOCK_STATUS + tattooSeatId, 1);
    }

    private void OnTattooSeatUnlocked(int id)
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.TATTOO_SELECTED_SEAT_ID + tattooSeatId, id);
        Instantiate(TattooSeatsManager.Instance.GetTattooSeat(id), tattooSeatParent);
    }
}
