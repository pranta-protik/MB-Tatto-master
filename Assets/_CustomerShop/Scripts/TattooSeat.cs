using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TattooSeat : MonoBehaviour
{
    [SerializeField] private int tattooSeatId;
    [SerializeField] private Transform tattooSeatParent;
    [SerializeField] private GameObject decorativeObject;
    [SerializeField] private Transform tattooArtistParent;
    [SerializeField] private TattooSeatSelectionPanel tattooSeatSelectionPanel;
    [SerializeField] private int unlockPrice;
    [SerializeField] private TattooSeatUnlockPlatform tattooSeatUnlockPlatform;
    [SerializeField] private Receptionist receptionist;
    [SerializeField] private Transform sittingPosition;
    [SerializeField] private GameObject tattooCustomer;
    
    public Action PaymentSuccessfulAction;

    public GameObject TattooCustomer
    {
        get => tattooCustomer;
        set => tattooCustomer = value;
    }
    public int UnlockPrice => unlockPrice;
    public bool IsUnlocked => _isUnlocked;

    public bool IsEmpty { get; private set; }
    
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
        Instantiate(TattooSeatsManager.Instance.GetRandomTattooArtist(), tattooArtistParent);

        IsEmpty = true;
        
        receptionist.AddRequestToQueue(this);
    }

    private void Start()
    {
        tattooSeatUnlockPlatform.Init(this);
        
        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0 ) == 0 && tattooSeatId != 1)
        {
            tattooSeatUnlockPlatform.gameObject.SetActive(false);
        }
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

        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0) == 0)
        {
            PointersManager.Instance.EnableNextPointer();    
        }
        
        _isUnlocked = true;
        PlayerPrefs.SetInt(PlayerPrefsKey.TATTOO_SEAT_UNLOCK_STATUS + tattooSeatId, 1);
    }

    private void OnTattooSeatUnlocked(int id)
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.TATTOO_SELECTED_SEAT_ID + tattooSeatId, id);
        Instantiate(TattooSeatsManager.Instance.GetTattooSeat(id), tattooSeatParent);
        Instantiate(TattooSeatsManager.Instance.GetRandomTattooArtist(), tattooArtistParent);

        IsEmpty = true;
        receptionist.AddRequestToQueue(this);
        
        tattooSeatSelectionPanel.TattooSeatUnlockAction -= OnTattooSeatUnlocked;
    }

    public void ApplyTattoo()
    {
        if (TattooCustomer != null)
        {
            TattooCustomer.transform.GetChild(0).LookAt(sittingPosition);
            
            TattooCustomer.transform.GetChild(0).GetComponent<CharacterUnlock>().anim.Play("Walking");
            TattooCustomer.transform.DOMove(sittingPosition.position, 3).SetEase(Ease.Linear).OnComplete(() =>
            {
                TattooCustomer.transform.GetChild(0).GetComponent<CharacterUnlock>().anim.Play("idle 0");
            });
        }
        
    }
}
