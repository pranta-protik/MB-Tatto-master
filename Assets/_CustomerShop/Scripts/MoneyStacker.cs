using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    bool HasMoney;

    void Start()
    {



        //  PlayerPrefs.SetInt("ArcadeCoin", InitialCoin);

        HasMoney = true;

        currencyText.text = StorageManager.GetTotalScore().ToString();
        

    }



    public void AddCoins(float ammount)
    {
        MainCoin += ammount;

        PlayerPrefs.SetFloat("ArcadeCoin", MainCoin);
        
        
 
    }
    public void RemoveCoins(int ammount)
    {
        int a = StorageManager.GetTotalScore();

        int k = a - ammount;
        currencyText.text = k.ToString();
        StorageManager.SetTotalScore(k);
    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            if (MainCoin >= 0)
            {
                m_GiveMoney = true;



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

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {


        }

    }

    public IEnumerator DecreaseStack(GameObject g)
    {
        yield return new WaitForSeconds(1f);
      

            

 


            yield return new WaitForSeconds(0f);


            yield return new WaitForSeconds(.3f);
          
            RemoveCoins(1);
            print("--");
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


