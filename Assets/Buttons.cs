using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Buttons : MonoBehaviour
{
    public int SelectedId;
    public Button CloseShop;
    public List<GameObject> Buttonss;
    public List<RectTransform> Positions;
    public GameObject Parent;

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
    public ButtonCard _selectedCard;
    private int _currentLevel;
    private float _distance;
    private bool _shouldPlayAnimation;

    private void Start()
    {
        actionButton.gameObject.SetActive(false);

        if (CloseShop != null)
        {
            CloseShop.onClick.AddListener(ShopClose);
        }
        if (scoreText != null)
        {
            scoreText.SetText("$" + StorageManager.GetTotalCoin());
        }

        _currentLevel = PlayerPrefs.GetInt("current_scene_text") + 1;

        if (levelNoText != null)
        {
            levelNoText.SetText("LEVEL " + _currentLevel);
        }

        for (int i = 0; i < Buttonss.Count; i++)
        {
             //Transform scrollViewTransform = transform;
            GameObject handCardObj = Instantiate(Buttonss[i], Positions[i].transform.position , Quaternion.identity , Parent.transform);
            ButtonCard handCard = handCardObj.GetComponent<ButtonCard>();

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
    }




    public void BuyCard()
    {
      
        StorageManager.SaveTotalCoin(StorageManager.GetTotalCoin() - Buttonss[SelectedId].GetComponent<ButtonCard>().requiredCash);
       // scoreText.SetText("$" + StorageManager.GetTotalCoin());
        Buttonss[SelectedId].GetComponent<ButtonCard>().EnableCard();
        EventSystem.current.currentSelectedGameObject.SetActive(false); // onclick button false
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

    public void ShopClose()
    {
       
     
        
   
        UiManager.Instance.Shop.gameObject.SetActive(true);

        Camera.main.transform.DOLocalRotate(new Vector3(27.761f, 90, 0), .1f).OnComplete(() => { UiManager.Instance.ShopPnael.SetActive(false); });

    }
    public void CheckCardRequirementStatus(ButtonCard btnCard)
    {
        if (btnCard.requirementType == ButtonCard.BRequirementType.Cash)
        {
            unlockInfoText.gameObject.SetActive(false);
            CheckActionButtonRequirement(btnCard);
        }
        else
        {
            actionButton.gameObject.SetActive(false);
        }

        if (btnCard.requirementType == ButtonCard.BRequirementType.Time)
        {
            if (PlayerPrefs.GetInt("TotalTime") / 60f >= btnCard.requiredTime)
            {
                btnCard.EnableCard();
            }
            else
            {
                btnCard.DisableCard();
            }

            CheckUnlockTextRequirement(btnCard,
                "Play game for <color=red>" + Mathf.RoundToInt(btnCard.requiredTime - PlayerPrefs.GetInt("TotalTime") / 60f) + "/" + btnCard.requiredTime +
                "</color> min Time");
        }

        if (btnCard.requirementType == ButtonCard.BRequirementType.GamePlay)
        {
            if (PlayerPrefs.GetInt("GameOpenCount") >= btnCard.requiredMatches)
            {
                btnCard.EnableCard();
            }
            else
            {
                btnCard.DisableCard();
            }

            CheckUnlockTextRequirement(btnCard,
                "Play the game <color=red>" + (btnCard.requiredMatches - PlayerPrefs.GetInt("GameOpenCount")) + "/" + btnCard.requiredMatches +
                "</color> times");
        }

        if (btnCard.requirementType == ButtonCard.BRequirementType.Level)
        {
            if (_currentLevel >= btnCard.requiredLevelNo)
            {
                btnCard.EnableCard();
            }
            else
            {
                btnCard.DisableCard();
            }
            CheckUnlockTextRequirement(btnCard, "Unlock after reaching level <color=red>" + btnCard.requiredLevelNo + "</color>");
        }
    }

    private void CheckActionButtonRequirement(ButtonCard btnCard)
    {
        if (PlayerPrefs.GetInt("HandCard" + btnCard.handId) == 0)
        {
            actionButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + btnCard.requiredCash);
            actionButton.gameObject.SetActive(true);
            DisableButton(startButton);
            if (StorageManager.GetTotalCoin() >= btnCard.requiredCash)
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

    private void CheckUnlockTextRequirement(ButtonCard handCard, string unlockText)
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


