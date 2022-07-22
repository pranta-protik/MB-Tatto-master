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
    [SerializeField] private float minDrawTime;
    [SerializeField] private float maxDrawTime;
    [SerializeField] private Transform[] customerExitPoints;
    [SerializeField] private CashGenerator cashGenerator;
    
    public Action PaymentSuccessfulAction;

    public GameObject TattooCustomer
    {
        get => tattooCustomer;
        set => tattooCustomer = value;
    }
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

    private TattooArtist _tattooArtist;
    private bool _isUnlocked;
    private int _currencyDeposited;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Sit = Animator.StringToHash("Sit");
    private static readonly int GetUp = Animator.StringToHash("GetUp");

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
        _tattooArtist = Instantiate(TattooSeatsManager.Instance.GetRandomTattooArtist(), tattooArtistParent).GetComponent<TattooArtist>();

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
        _tattooArtist = Instantiate(TattooSeatsManager.Instance.GetRandomTattooArtist(), tattooArtistParent).GetComponent<TattooArtist>();
        
        receptionist.AddRequestToQueue(this);
        
        tattooSeatSelectionPanel.TattooSeatUnlockAction -= OnTattooSeatUnlocked;
    }

    public void ApplyTattoo()
    {
        if (TattooCustomer != null)
        {
            TattooCustomer.transform.LookAt(sittingPosition);
            TattooCustomer.transform.GetChild(0).LookAt(sittingPosition);
            
            TattooCustomer.transform.GetChild(0).GetComponent<Animator>().SetBool(IsWalking, true);
            TattooCustomer.transform.DOMove(sittingPosition.position, 3).SetEase(Ease.Linear).OnComplete(() =>
            {
                TattooCustomer.transform.GetChild(0).GetComponent<Animator>().SetBool(IsWalking, false);
                TattooCustomer.transform.DORotate(new Vector3(0f, 180f, 0f), 0).OnComplete(() =>
                {
                    TattooCustomer.transform.GetChild(0).GetComponent<Animator>().SetTrigger(Sit);
                    Invoke(nameof(LayCustomer), 1f);
                });
            });
        }
    }

    private void LayCustomer()
    {
        TattooCustomer.transform.GetChild(0).DOLocalMove(new Vector3(0.35f, 0f, -0.5f), 0f);
        TattooCustomer.transform.GetChild(0).DOLocalRotate(Vector3.zero, 0f);
        
        DrawTattoo();
    }

    private void DrawTattoo()
    {
        _tattooArtist.StartDrawingTattoo();
        float drawTime = Random.Range(minDrawTime, maxDrawTime);
        Invoke(nameof(RemoveCustomer), drawTime);
    }

    private void RemoveCustomer()
    {
        _tattooArtist.StopDrawingTattoo();
        TattooCustomer.transform.GetChild(0).GetComponent<Animator>().SetTrigger(GetUp);
        Invoke(nameof(MoveCustomerToExit), 0.8f);
        cashGenerator.GenerateStack();
    }

    private void MoveCustomerToExit()
    {
        TattooCustomer.GetComponent<Customer>().Move(customerExitPoints);
        TattooCustomer = null;
        
        receptionist.AddRequestToQueue(this);
    }
}