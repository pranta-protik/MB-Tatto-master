using TMPro;
using UnityEngine;

public class CurrencySimpleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private const string DOLAR = "$";
    
    private void Awake()
    {
        StorageManager.Instance.CurrentScoreChangedAction += OnCurrencyChanged;
    }

    private void OnDestroy()
    {
        StorageManager.Instance.CurrentScoreChangedAction -= OnCurrencyChanged;
    }
    
    private void OnCurrencyChanged(int currency)
    {
        currencyText.SetText(string.Concat(DOLAR, currency));
    }
}
