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
    private HandCardOld _selectedCardOld;
    private int _currentLevel;
    private float _distance;
    private bool _shouldPlayAnimation;
    
    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.SetText("$" + StorageManager.GetTotalCoin());
        }
        
        _currentLevel = PlayerPrefs.GetInt("current_scene_text") + 1;
      
        if (levelNoText != null)
        {
            levelNoText.SetText("LEVEL " + _currentLevel);   
        }

        for (int i = 0; i < handCards.Count; i++)
        {
            Transform scrollViewTransform = transform;
            GameObject handCardObj = Instantiate(handCards[i], scrollViewTransform.position, Quaternion.identity, scrollViewTransform);
            HandCardOld handCardOld = handCardObj.GetComponent<HandCardOld>();
            
            if (PlayerPrefs.GetInt("HandCardSetup", 0) == 0)
            {
                Debug.Log("First Time");
             
                if (i == 0)
                {
                    PlayerPrefs.SetInt("HandCard" + handCardOld.handId, 1);    
                }
                else
                {
                    PlayerPrefs.SetInt("HandCard" + handCardOld.handId, 0);
                }
                
            }
            handCardOld.cardStatus = PlayerPrefs.GetInt("HandCard" + handCardOld.handId);
        }

        PlayerPrefs.SetInt("HandCardSetup", 1);
        
        _pos = new float[transform.childCount];
        _distance = 1f / (_pos.Length - 1f);
        
        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = _distance * i;
        }

        int selectedHandId = 0;

        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i].GetComponent<HandCardOld>().handId == PlayerPrefs.GetInt("SelectedHandId"))
            {
                selectedHandId = i;
                break;
            }
        }
        
        _scrollPos = _distance * selectedHandId;
    }

    private void Update()
    {
        ScrollCards();
    }

    private void ScrollCards()
    {
        if (Input.GetMouseButton(0))
        {
            _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < _pos.Length; i++)
        {
            if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
            {
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, new Vector3(1f, 1f, 1f), 0.1f);
                _selectedCardOld = transform.GetChild(i).GetComponent<HandCardOld>();
                
                _selectedCardOld.shineEffect.SetActive(true);
                
                if (_selectedCardOld.cardType == HandCardOld.ECardType.Model)
                {
                    if (_selectedCardOld.handId != PlayerPrefs.GetInt("SelectedHandId"))
                    {
                        _shouldPlayAnimation = true;
                    }

                    if (_shouldPlayAnimation)
                    {
                        _selectedCardOld.PlayRandomAnimation();   
                    }
                }
                CheckCardRequirementStatus(_selectedCardOld);
                
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector3.Lerp(transform.GetChild(j).localScale, new Vector3(0.6f, 0.6f, 0.6f), 0.1f);
                        
                        HandCardOld unselectedCardOld = transform.GetChild(j).GetComponent<HandCardOld>();
                        
                        unselectedCardOld.shineEffect.SetActive(false);
                        
                        if (unselectedCardOld.cardType == HandCardOld.ECardType.Model)
                        {
                            unselectedCardOld.PlayIdleAnimation();   
                        }
                    }
                }
            }
        }
    }

    public void BuyCard()
    {
        StorageManager.SaveTotalCoin(StorageManager.GetTotalCoin() - _selectedCardOld.requiredCash);
        scoreText.SetText("$" + StorageManager.GetTotalCoin());
        _selectedCardOld.EnableCard();
    }
    
    public void SelectHand()
    {
        PlayerPrefs.SetInt("SelectedHandId", _selectedCardOld.handId);
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

   
    public void CheckCardRequirementStatus(HandCardOld handCardOld)
    {
        if (_selectedCardOld.requirementType == HandCardOld.ERequirementType.Cash)
        {
            unlockInfoText.gameObject.SetActive(false);
            CheckActionButtonRequirement(handCardOld);
        }
        else
        {
            actionButton.gameObject.SetActive(false);
        }

        if (_selectedCardOld.requirementType == HandCardOld.ERequirementType.Time)
        {
            if (PlayerPrefs.GetInt("TotalTime") / 60f >= handCardOld.requiredTime)
            {
                handCardOld.EnableCard();
            }
            else
            {
                handCardOld.DisableCard();
            }

            CheckUnlockTextRequirement(handCardOld,
                "Play game for <color=red>" + Mathf.RoundToInt(handCardOld.requiredTime - PlayerPrefs.GetInt("TotalTime") / 60f) + "/" + handCardOld.requiredTime +
                "</color> min Time");
        }

        if (_selectedCardOld.requirementType == HandCardOld.ERequirementType.GamePlay)
        {
            if (PlayerPrefs.GetInt("GameOpenCount") >= handCardOld.requiredMatches)
            {
                handCardOld.EnableCard();
            }
            else
            {
                handCardOld.DisableCard();
            }

            CheckUnlockTextRequirement(handCardOld,
                "Play the game <color=red>" + (handCardOld.requiredMatches - PlayerPrefs.GetInt("GameOpenCount")) + "/" + handCardOld.requiredMatches +
                "</color> times");
        }

        if (_selectedCardOld.requirementType == HandCardOld.ERequirementType.Level)
        {
            if (_currentLevel >= handCardOld.requiredLevelNo)
            {
                handCardOld.EnableCard();
            }
            else
            {
                handCardOld.DisableCard();
            }
            CheckUnlockTextRequirement(handCardOld, "Unlock after reaching level <color=red>" + handCardOld.requiredLevelNo + "</color>");
        }
    }
    
    private void CheckActionButtonRequirement(HandCardOld handCardOld)
    {
        if (PlayerPrefs.GetInt("HandCard" + handCardOld.handId) == 0)
        {
            actionButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + handCardOld.requiredCash);
            actionButton.gameObject.SetActive(true);
            DisableButton(startButton);
            if (StorageManager.GetTotalCoin() >= handCardOld.requiredCash)
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

    private void CheckUnlockTextRequirement(HandCardOld handCardOld, string unlockText)
    {
        if (PlayerPrefs.GetInt("HandCard" + handCardOld.handId) == 0)
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
