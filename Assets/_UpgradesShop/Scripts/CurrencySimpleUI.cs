using TMPro;
using UnityEngine;

public class CurrencySimpleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Awake()
    {
        StorageManager.CurrentScoreChangedAction += OnCurrencyChanged;
    }

    private void Start()
    {
        SetCurrency(StorageManager.GetTotalScore());
    }

    private void OnDestroy()
    {
        StorageManager.CurrentScoreChangedAction -= OnCurrencyChanged;
    }

    private void OnCurrencyChanged(int currency)
    {
        SetCurrency(currency);
    }

    private void SetCurrency(int currency)
    {
        currencyText.SetText($"${CurrencySystem.GetConvertedCurrencyString(currency)}");
    }
}