using UnityEngine;
using TMPro;
public class TipJar : MonoBehaviour
{
    [SerializeField] private GameObject fullCashJar;
    [SerializeField] private GameObject emptyCashJar;
    [SerializeField] private TMP_Text cashJarText;
    [SerializeField] private int[] randomCashAmounts;

    private int _jarCashAmount;
    
    private void Awake()
    {
        int randomIndex = Random.Range(0, randomCashAmounts.Length);

        _jarCashAmount = randomCashAmounts[randomIndex];
        cashJarText.SetText($"${_jarCashAmount}");
        
        fullCashJar.SetActive(true);
        emptyCashJar.SetActive(false);
    }

    public void CollectCashFromJar()
    {
        fullCashJar.SetActive(false);
        emptyCashJar.SetActive(true);
        cashJarText.transform.parent.gameObject.SetActive(false);
        StorageManager.AddToTotalScore(_jarCashAmount);
    }
}



