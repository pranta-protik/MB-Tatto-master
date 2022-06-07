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

    [Header("Requirement Section")] public BCardType cardType;
    public BRequirementType requirementType;
    public int handId;
    public int requiredCash;
    public float requiredTime;
    public int requiredMatches;
    public int requiredLevelNo;
    [HideInInspector] public int cardStatus;

    public int isUnlockable;

    public bool isNotified;
    // ButtonCard b;
    [HideInInspector] public GameObject shineEffect;
    public bool Unlocked;

    private static readonly int IsSelected = Animator.StringToHash("isSelected");

    Buttons m_Buttons;

    private void Start()
    {
        // b = GetComponent<ButtonCard>();
        m_Buttons = GetComponentInParent<Buttons>();
        if (handId == PlayerPrefs.GetInt("SelectedHandId"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
        m_Buttons.CheckCardStatus(this);

        if (handId == 0)
        {
            PlayerPrefs.SetInt("IsUnlockable" + handId, 1);
            PlayerPrefs.SetInt("IsNotified" + handId, 1);
        }
        else
        {
            isUnlockable = PlayerPrefs.GetInt("IsUnlockable" + handId, 0);
            if (isUnlockable == 1)
            {
                if (PlayerPrefs.GetInt("IsNotified" + handId, 0) == 0)
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    public int index;

    public void GetSelectedId()
    {
        if (isUnlockable == 1)
        {
            PlayerPrefs.SetInt("IsNotified" + handId, 1);   
        }
        m_Buttons.SelectedId = handId;
        transform.GetChild(1).gameObject.SetActive(false);
        GameManager.Instance.SpawnHand(handId);
        m_Buttons.CheckCardRequirementStatus(this);
        index = m_Buttons.SelectedId;
        for (int i = 0; i < m_Buttons.SpawnedButtons.Count; i++)
        {
            if (i == index)
            {

                m_Buttons.SpawnedButtons[i].transform.GetChild(0).gameObject.SetActive(true);

            }
            else
            {
                m_Buttons.SpawnedButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
        if (cardStatus == 1)
        {
            PlayerPrefs.SetInt("SelectedHandId", m_Buttons.SelectedId);
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
