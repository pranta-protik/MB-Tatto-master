using TMPro;
using UnityEngine;

public class CurrencySimpleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private const string DOLAR = "$";

    private void Awake()
    {
        StorageManager.CurrentScoreChangedAction += OnCurrencyChanged;
    }

    private void Start()
    {
        SetCurrency(StorageManager.GetTotalScore().ToString());
    }

    private void OnDestroy()
    {
        StorageManager.CurrentScoreChangedAction -= OnCurrencyChanged;
    }

    private void OnCurrencyChanged(int currency)
    {
        SetCurrency(currency.ToString());
    }

    private void SetCurrency(string currency)
    {
        currencyText.SetText(string.Concat(DOLAR, currency));
    }
}