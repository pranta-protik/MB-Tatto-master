using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public List<GameObject> handCards = new List<GameObject>();
    public GameObject scrollbar;
    public Button startButton;
    public Button actionButton;
    public TextMeshProUGUI unlockInfoText;
    private float _scrollPos = 0;
    private float[] _pos;
    private HandCard _selectedCard;
    [SerializeField] private string currentSelection;

    private void Start()
    {
        foreach (GameObject handCard in handCards)
        {
            Transform scrollViewTransform = transform;
            Instantiate(handCard, scrollViewTransform.position, Quaternion.identity, scrollViewTransform);
        }
    }

    private void Update()
    {
        ScrollCards();
    }

    public void SelectHand()
    {
        Debug.Log(_selectedCard.handId);
        Debug.Log(_selectedCard.requirementType);
    }
    
    private void ScrollCards()
    {
        _pos = new float[transform.childCount];
        float distance = 1f / (_pos.Length - 1f);
        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < _pos.Length; i++)
        {
            if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, new Vector3(1f, 1f, 1f), 0.1f);
                _selectedCard = transform.GetChild(i).GetComponent<HandCard>();
                _selectedCard.PlayRandomAnimation();
                CheckCardRequirementStatus(_selectedCard);
                
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector3.Lerp(transform.GetChild(j).localScale, new Vector3(0.6f, 0.6f, 0.6f), 0.1f);
                        transform.GetChild(j).GetComponent<HandCard>().PlayIdleAnimation();
                    }
                }
            }
        }
    }

    public void BuyCard()
    {
        _selectedCard.UpdateCardStatus();
    }
    
    private void CheckCardRequirementStatus(HandCard handCard)
    {
        if (_selectedCard.requirementType == HandCard.ERequirementType.Cash)
        {
            unlockInfoText.gameObject.SetActive(false);
            
            if (PlayerPrefs.GetInt("HandCard" + handCard.handId) == 0)
            {
                actionButton.gameObject.SetActive(true);
                startButton.interactable = false;
                if (StorageManager.GetTotalCoin() >= handCard.requiredCash)
                {
                    actionButton.interactable = true;   
                }
                else
                {
                    actionButton.interactable = false;
                }
            }
            else
            {
                actionButton.gameObject.SetActive(false);
                startButton.interactable = true;
            }
        }
        else
        {
            actionButton.gameObject.SetActive(false);
        }

        if (_selectedCard.requirementType == HandCard.ERequirementType.Time)
        {
            CheckUnlockTextRequirement(handCard, "Play game for " + handCard.requiredTime + "/" + handCard.requiredTime + " min Time");
        }

        if (_selectedCard.requirementType == HandCard.ERequirementType.GamePlay)
        {
            CheckUnlockTextRequirement(handCard, "Play the game " + handCard.requiredMatches + "/" + handCard.requiredMatches + " times");
        }

        if (_selectedCard.requirementType == HandCard.ERequirementType.Level)
        {
            CheckUnlockTextRequirement(handCard, "Unlock after reaching level " + handCard.requiredLevelNo);
        }
    }

    private void CheckUnlockTextRequirement(HandCard handCard, string unlockText)
    {
        if (PlayerPrefs.GetInt("HandCard" + handCard.handId) == 0)
        {
            unlockInfoText.gameObject.SetActive(true);
            unlockInfoText.SetText(unlockText);
            startButton.interactable = false;
        }
        else
        {
            startButton.interactable = true;
        }
    }
}
