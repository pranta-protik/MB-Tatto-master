using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonCard : MonoBehaviour
{
    public enum BCardType
    {
        Model,
        Image
    }
    public enum BRequirementType
    {
        Cash,
        Time,
        GamePlay,
        Level
    }

    [Header("Requirement Section")]
    public BCardType cardType;
    public BRequirementType requirementType;
    public int handId;
    public int requiredCash;
    public float requiredTime;
    public int requiredMatches;
    public int requiredLevelNo;
    public int cardStatus;
    ButtonCard b;
    [HideInInspector] public GameObject shineEffect;
    public bool Unlocked;

    private static readonly int IsSelected = Animator.StringToHash("isSelected");

    Buttons m_Buttons;

    private void Start()
    {
        b = GetComponent<ButtonCard>();
        m_Buttons = GetComponentInParent<Buttons>();
       transform.GetChild(0).gameObject.SetActive(false);

    }
   public int index;
    public void GetSelectedId()
    {



        m_Buttons.SelectedId = handId;
        GameManager.Instance.SpawnHand(handId);
        m_Buttons.CheckCardRequirementStatus(b);
        index = m_Buttons.SelectedId;
        for (int i = 0; i < m_Buttons.SpawnedButtons.Count; i++)

        {
           
            if(i== index)
            {

                m_Buttons.SpawnedButtons[i].transform.GetChild(0).gameObject.SetActive(true);
              
            }
            else
            {
               m_Buttons.SpawnedButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
         

        
       



    }

    public void EnableCard()
    {
        cardStatus = 1;
        PlayerPrefs.SetInt("HandCard" + handId, cardStatus);

    }

    public void DisableCard()
    {
        cardStatus = 0;
        PlayerPrefs.SetInt("HandCard" + handId, cardStatus);
    }

}
