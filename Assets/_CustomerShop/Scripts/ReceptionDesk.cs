using System;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionDesk : MonoBehaviour
{
    [SerializeField] private int unlockPrice;
    [SerializeField] private List<Renderer> renderersToBlack = new List<Renderer>();
    [SerializeField] private ReceptionUnlockPlatform receptionUnlockPlatform;
    [SerializeField] private GameObject receptionist;
    [SerializeField] private ParticleSystem receptionUnlockEffect;

    public Action PaymentSuccessfulAction;
    public int UnlockPrice => unlockPrice;
    public bool IsUnlocked => _isUnlocked;
    
    public int CurrencyDeposited
    {
        get => _currencyDeposited;
        private set
        {
            _currencyDeposited = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.RECEPTION_DESK_CURRENCY_DEPOSITED, _currencyDeposited);
        }
    }

    private bool _isUnlocked;
    private int _currencyDeposited;
    private List<Texture2D> _originalTextures;
    private List<Color> _originalColors;
    private bool _hasUsedNullTexture;

    private void Awake()
    {
        _currencyDeposited = PlayerPrefs.GetInt(PlayerPrefsKey.RECEPTION_DESK_CURRENCY_DEPOSITED, 0);
        _isUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey.RECEPTION_DESK_UNLOCK_STATUS, 0) == 1;
        
        if (!_isUnlocked)
        {
            SetUnlockStatus(true);
            receptionist.SetActive(false);
            return;
        }
        
        receptionist.SetActive(true);
        SetUnlockStatus(false);
    }

    private void Start()
    {
        receptionUnlockPlatform.Init(this);
    }

    private void SetUnlockStatus(bool isLocked)
    {
        if (isLocked)
        {
            _originalTextures = new List<Texture2D>();
            _originalColors = new List<Color>();

            for (int i = 0, count = renderersToBlack.Count; i < count; i++)
            {
                foreach (var rendererMaterial in renderersToBlack[i].materials)
                {
                    _originalTextures.Add(rendererMaterial.mainTexture as Texture2D);
                    _originalColors.Add(rendererMaterial.color);
                    
                    rendererMaterial.mainTexture = null;
                    rendererMaterial.color = Color.black;
                }
            }

            _hasUsedNullTexture = true;
        }
        else if (_hasUsedNullTexture)
        {
            for (int i = 0, count = renderersToBlack.Count; i < count; i++)
            {
                foreach (var rendererMaterial in renderersToBlack[i].materials)
                {
                    rendererMaterial.mainTexture = _originalTextures[0];
                    _originalTextures.RemoveAt(0);
                    rendererMaterial.color = _originalColors[0];
                    _originalColors.RemoveAt(0);
                }
            }

            _hasUsedNullTexture = false;
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
        PaymentSuccessfulAction?.Invoke();
        SetUnlockStatus(false);
        receptionUnlockEffect.Play();
        receptionist.SetActive(true);

        if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0) == 0)
        {
            PointersManager.Instance.EnableNextPointer();    
        }
        
        _isUnlocked = true;
        PlayerPrefs.SetInt(PlayerPrefsKey.RECEPTION_DESK_UNLOCK_STATUS, 1);
    }
}