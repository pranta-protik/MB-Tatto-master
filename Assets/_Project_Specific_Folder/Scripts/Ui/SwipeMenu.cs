using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public List<GameObject> handCards = new List<GameObject>();
    public GameObject scrollbar;
    public TextMeshProUGUI levelNoText;
    public Button startButton;
    public Button actionButton;
    public float buttonDisabledAlpha = 0.5f;
    public TextMeshProUGUI unlockInfoText;
    public TextMeshProUGUI scoreText;
    private float _scrollPos = 0;
    private float[] _pos;
    private HandCard _selectedCard;
    private int _currentLevel;
    
    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.SetText(StorageManager.GetTotalCoin().ToString());
        }
        
        _currentLevel = PlayerPrefs.GetInt("current_scene_text") + 1;
      
        if (levelNoText != null)
        {
            levelNoText.SetText("Level - " + _currentLevel);   
        }

        for (int i = 0; i < handCards.Count; i++)
        {
            Transform scrollViewTransform = transform;
            GameObject handCardObj = Instantiate(handCards[i], scrollViewTransform.position, Quaternion.identity, scrollViewTransform);
            HandCard handCard = handCardObj.GetComponent<HandCard>();
            
            if (PlayerPrefs.GetInt("HandCardSetup", 0) == 0)
            {
                Debug.Log("First Time");
             
                if (i == 0)
                {
                    PlayerPrefs.SetInt("HandCard" + handCard.handId, 1);    
                }
                else
                {
                    PlayerPrefs.SetInt("HandCard" + handCard.handId, 0);
                }
                
            }
            handCard.cardStatus = PlayerPrefs.GetInt("HandCard" + handCard.handId);
        }

        PlayerPrefs.SetInt("HandCardSetup", 1);
        
        // scrollbar.GetComponent<Scrollbar>().value 
    }

    private void Update()
    {
        ScrollCards();
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
                
                if (_selectedCard.cardType == HandCard.ECardType.Model)
                {
                    _selectedCard.PlayRandomAnimation();
                }
                CheckCardRequirementStatus(_selectedCard);
                
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector3.Lerp(transform.GetChild(j).localScale, new Vector3(0.6f, 0.6f, 0.6f), 0.1f);
                        
                        HandCard unselectedCard = transform.GetChild(j).GetComponent<HandCard>();
                        
                        if (unselectedCard.cardType == HandCard.ECardType.Model)
                        {
                            unselectedCard.PlayIdleAnimation();   
                        }
                    }
                }
            }
        }
    }

    public void BuyCard()
    {
        StorageManager.SaveTotalCoin(StorageManager.GetTotalCoin() - _selectedCard.requiredCash);
        scoreText.SetText(StorageManager.GetTotalCoin().ToString());
        _selectedCard.EnableCard();
    }
    
    public void SelectHand()
    {
        PlayerPrefs.SetInt("SelectedHandId", _selectedCard.handId);
        UiManager.Instance.Next();
    }
    
    private void DisableButton(Button button)
    {
        button.interactable = false;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(buttonDisabledAlpha, 0.01f);
        button.transform.GetComponent<Image>().DOFade(buttonDisabledAlpha, 0.01f);
    }

    private void EnableButton(Button button)
    {
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1f, 0.01f);
        button.transform.GetComponent<Image>().DOFade(1f, 0.01f).OnComplete(() =>
        {
            button.interactable = true;
        });
    }

   
    private void CheckCardRequirementStatus(HandCard handCard)
    {
        if (_selectedCard.requirementType == HandCard.ERequirementType.Cash)
        {
            unlockInfoText.gameObject.SetActive(false);
            CheckActionButtonRequirement(handCard);
        }
        else
        {
            actionButton.gameObject.SetActive(false);
        }

        if (_selectedCard.requirementType == HandCard.ERequirementType.Time)
        {
            if (PlayerPrefs.GetInt("TotalTime") >= handCard.requiredTime)
            {
                handCard.EnableCard();
            }
            else
            {
                handCard.DisableCard();
            }

            CheckUnlockTextRequirement(handCard,
                "Play game for <color=red>" + (handCard.requiredTime - PlayerPrefs.GetInt("TotalTime")) + "/" + handCard.requiredTime + "</color> min Time");
        }

        if (_selectedCard.requirementType == HandCard.ERequirementType.GamePlay)
        {
            if (PlayerPrefs.GetInt("GameOpenCount") >= handCard.requiredMatches)
            {
                handCard.EnableCard();
            }
            else
            {
                handCard.DisableCard();
            }

            CheckUnlockTextRequirement(handCard,
                "Play the game <color=red>" + (handCard.requiredMatches - PlayerPrefs.GetInt("GameOpenCount")) + "/" + handCard.requiredMatches +
                "</color> times");
        }

        if (_selectedCard.requirementType == HandCard.ERequirementType.Level)
        {
            if (_currentLevel >= handCard.requiredLevelNo)
            {
                handCard.EnableCard();
            }
            else
            {
                handCard.DisableCard();
            }
            CheckUnlockTextRequirement(handCard, "Unlock after reaching level <color=red>" + handCard.requiredLevelNo + "</color>");
        }
    }
    
    private void CheckActionButtonRequirement(HandCard handCard)
    {
        if (PlayerPrefs.GetInt("HandCard" + handCard.handId) == 0)
        {
            actionButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + handCard.requiredCash);
            actionButton.gameObject.SetActive(true);
            DisableButton(startButton);
            if (StorageManager.GetTotalCoin() >= handCard.requiredCash)
            {
                EnableButton(actionButton);
            }
            else
            {
                DisableButton(actionButton);
            }
        }
        else
        {
            actionButton.gameObject.SetActive(false);
            EnableButton(startButton);
        }
    }

    private void CheckUnlockTextRequirement(HandCard handCard, string unlockText)
    {
        if (PlayerPrefs.GetInt("HandCard" + handCard.handId) == 0)
        {
            unlockInfoText.gameObject.SetActive(true);
            unlockInfoText.SetText(unlockText);
            DisableButton(startButton);
        }
        else
        {
            unlockInfoText.gameObject.SetActive(false);
            EnableButton(startButton);
        }
    }
}
