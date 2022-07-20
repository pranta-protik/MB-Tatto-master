using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using TMPro;

public class MoneyStacker : MonoBehaviour
{
    public float InitialCoin;
    public float MainCoin;
    public int StackAmmount;
    public GameObject MoneyPrefab;
    public float Increaseammount;
    public GameObject Parent;

    [SerializeField] public float NextYpos;

    [SerializeField] bool m_GiveMoney;

    [SerializeField] public List<GameObject> MoneyStack = new List<GameObject>();

    public GameObject TrashCan;
    public int GiveAmmount;
    public float NextPosCap;
    public int DummyNumber;
    public TextMeshProUGUI currencyText;
    private bool HasMoney;
    public GameObject CashUi;
    int CashAmmount;

    private float _time;
    private bool isProgressBarFilling;

    [SerializeField] private float second;
    private float elapsedTime;
    private bool isPaymentOngoing = false;
    [SerializeField] private float timeToPay = 0.4f;

    void Start()
    {
        StorageManager.SetTotalScore(StorageManager.GetTotalScore() + 88852);

        //  PlayerPrefs.SetInt("ArcadeCoin", InitialCoin);

        HasMoney = true;

        currencyText.text = StorageManager.GetTotalScore().ToString();
    }

    public void AddCoins(float amount)
    {
        MainCoin += amount;
        PlayerPrefs.SetFloat("ArcadeCoin", MainCoin);
    }

    public void RemoveCoins(int amount, Shop shop)
    {
        if (shop.BillboardCost > 0)
        {
            shop.BillboardCost -= amount;
        }
         
        shop.BillBoardText.text = $"${shop.BillboardCost}";
        
        if (StorageManager.GetTotalScore() <= 0)
        {
            StorageManager.SetTotalScore(0);
        }
        StorageManager.SetTotalScore(StorageManager.GetTotalScore() - amount);
        currencyText.text = StorageManager.GetTotalScore().ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            if (isProgressBarFilling)
            {
                if (_time < second)
                {
                    _time += Time.deltaTime;
                    other.GetComponent<Shop>().progressBar.fillAmount = _time / second;
                }
                else
                {
                    isPaymentOngoing = true;
                    isProgressBarFilling = false;
                }
            }

            if (!isProgressBarFilling && isPaymentOngoing)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= timeToPay)
                {
                    elapsedTime -= timeToPay;
                    m_GiveMoney = true;
                }
            }

            if (StorageManager.GetTotalScore() >= 0)
            {
                if (!m_GiveMoney)
                    StopCoroutine(DecreaseStack(other.gameObject));
                else
                    StartCoroutine(DecreaseStack(other.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            m_GiveMoney = false;
            isProgressBarFilling = false;
            other.GetComponent<Shop>().progressBar.fillAmount = 0;

            isPaymentOngoing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            isProgressBarFilling = true;
            isPaymentOngoing = false;
            elapsedTime = 0f;
            _time = 0;
            //  other.GetComponent<Shop>().InstantUnlock();
        }
        if (other.gameObject.CompareTag("Cash"))
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            
            int roll = Random.Range(1, 4);
            other.transform.GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(0).gameObject.SetActive(true);
            if (roll == 2) CashAmmount = 100;
            if (roll == 3) CashAmmount = 200;
            if (roll == 1) CashAmmount = 500;
            StorageManager.SetTotalScore(StorageManager.GetTotalScore() + CashAmmount);
            CashUi.transform.DOScale(CashUi.transform.localScale * 1.1f, .1f).OnComplete(() =>
            {
                CashUi.transform.DOScale(new Vector3(2f, 2.17f, 1.5f), .1f);
            });
            Destroy(other.gameObject, 1);
        }
        // if (other.gameObject.CompareTag("Jarr"))
        // {
        //
        // }
    }
    
    public IEnumerator DecreaseStack(GameObject g)
    {

        yield return new WaitForSeconds(.3f);

        RemoveCoins(1, g.GetComponent<Shop>());

        g.GetComponent<Shop>().AddMoney(1);
    }
    
    public void DelayCll()
    {
        if (MoneyStack != null)
        {
            for (int j = 0; j < MoneyStack.Count; j++)
            {
                Destroy(MoneyStack[j].gameObject);
            }
            MoneyStack.Clear();
            NextYpos = 0;
        }
    }
}