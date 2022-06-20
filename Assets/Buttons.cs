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
    public List<GameObject> SpawnedButtons;
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
    private new Camera _camera;

    private void Start()
    {
        _camera = Camera.main;

        actionButton.gameObject.SetActive(false);

        if (CloseShop != null)
        {
            CloseShop.onClick.AddListener(ShopClose);
        }

        if (scoreText != null)
        {
            scoreText.SetText("$" + StorageManager.GetTotalScore());
        }

        _currentLevel = PlayerPrefs.GetInt("current_scene_text") + 1;

        if (levelNoText != null)
        {
            levelNoText.SetText("LEVEL " + _currentLevel);
        }

        for (int i = 0; i < Buttonss.Count; i++)
        {
            //Transform scrollViewTransform = transform;
            GameObject handCardObj = Instantiate(Buttonss[i], Positions[i].transform.position, Quaternion.identity, Parent.transform);
            ButtonCard handCard = handCardObj.GetComponent<ButtonCard>();
            SpawnedButtons.Add(handCardObj);

            //SpawnedButtons[PlayerPrefs.GetInt("SelectedHandId")].transform.GetChild(0).gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("HandCardSetup", 0) == 0)
            {
                PlayerPrefs.SetInt("SelectedHandId", 0);

                Debug.Log("First Time");
                SpawnedButtons[0].transform.GetChild(0).gameObject.SetActive(true);
                if (i == 0)
                {
                    PlayerPrefs.SetInt("HandCard" + handCard.handId, 1);
                }
                else
                {
                    PlayerPrefs.SetInt("HandCard" + handCard.handId, 0);
                }

            }
            //else if(PlayerPrefs.GetInt("HandCardSetup", 0) == 01)
            // {
            //     print("ddfssdf");
            //     int k = PlayerPrefs.GetInt("SelectedHandId");

            //     if (k == 0)
            //     {
            //         PlayerPrefs.SetInt("SelectedHandId" ,0);
            //         SpawnedButtons[PlayerPrefs.GetInt("SelectedHandId")].transform.GetChild(0).gameObject.SetActive(true);
            //     }
            //     else if (k > 0)
            //     {
            //         SpawnedButtons[PlayerPrefs.GetInt("SelectedHandId")].transform.GetChild(0).gameObject.SetActive(true);
            //     }
            // }
            handCard.cardStatus = PlayerPrefs.GetInt("HandCard" + handCard.handId);
        }

        PlayerPrefs.SetInt("HandCardSetup", 1);
    }

    public void BuyCard()
    {

        StorageManager.SetTotalScore(StorageManager.GetTotalScore() - Buttonss[SelectedId].GetComponent<ButtonCard>().requiredCash);
        scoreText.SetText("$" + StorageManager.GetTotalScore());
        SpawnedButtons[SelectedId].GetComponent<ButtonCard>().EnableCard();

        PlayerPrefs.SetInt("SelectedHandId", Buttonss[SelectedId].GetComponent<ButtonCard>().handId);
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
        button.transform.GetComponent<Image>().DOFade(1f, 0.01f).OnComplete(() => { button.interactable = true; });
    }

    private void ShopClose()
    {
        foreach (GameObject handButton in SpawnedButtons)
        {
            ButtonCard handCard = handButton.GetComponent<ButtonCard>();
            if (handCard.handId == PlayerPrefs.GetInt("SelectedHandId"))
            {
                handButton.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                handButton.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        GameManager.Instance.SpawnHand(PlayerPrefs.GetInt("SelectedHandId"));
        // UiManager.Instance.hand.gameObject.SetActive(true);
        // UiManager.Instance.shop.SetActive(true);
        // UiManager.Instance.ShopPnael.SetActive(false);
        _camera.transform.DOLocalRotate(new Vector3(27.761f, 90, 0), .3f).OnComplete(() => { });
    }

    public void CheckCardStatus(ButtonCard btnCard)
    {
        if (btnCard.requirementType == ButtonCard.BRequirementType.Cash)
        {
            if (StorageManager.GetTotalScore() >= btnCard.requiredCash)
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 0);
            }
        }

        if (btnCard.requirementType == ButtonCard.BRequirementType.Time)
        {
            if (PlayerPrefs.GetInt("TotalTime") / 60f >= btnCard.requiredTime)
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 0);
            }
        }

        if (btnCard.requirementType == ButtonCard.BRequirementType.GamePlay)
        {
            if (PlayerPrefs.GetInt("GameOpenCount") >= btnCard.requiredMatches)
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 0);
            }
        }

        if (btnCard.requirementType == ButtonCard.BRequirementType.Level)
        {
            if (_currentLevel >= btnCard.requiredLevelNo)
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsUnlockable"+btnCard.handId, 0);
            }
        }
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
                "Play game for <color=red>" + Mathf.Ceil(btnCard.requiredTime - PlayerPrefs.GetInt("TotalTime") / 60f) + "/" + btnCard.requiredTime +
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
            if (StorageManager.GetTotalScore() >= btnCard.requiredCash)
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