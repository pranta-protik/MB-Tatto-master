using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectionMenu : MonoBehaviour
{
    public GameObject handCardPrefab;
    public List<Sprite> handCardTextures = new List<Sprite>();
    public List<GameObject> handCards = new List<GameObject>();
    public int baseUnlockCost;
    private int _multiplier;
    private GameObject _unlockButton;
    private int _totalCards;
    private void Awake()
    {
        _unlockButton = transform.GetChild(3).gameObject;

        Vector3 spawnPosition = transform.GetChild(2).GetChild(0).position;
        Vector2 anchoredSpawnPosition = transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchoredPosition;

        _totalCards = handCardTextures.Count;
        for (int i = 0; i < _totalCards; i++)
        {
            GameObject handCard = Instantiate(handCardPrefab, spawnPosition, Quaternion.identity, transform.GetChild(2));

            handCard.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(anchoredSpawnPosition.x + ((i % 3) * 270), anchoredSpawnPosition.y - ((i / 3) * 230));
            
            handCard.transform.GetChild(1).GetComponent<Image>().sprite = handCardTextures[i];
            handCard.GetComponent<HandCard>().handId = i;
            
            handCards.Add(handCard);
        }
    }

    public void CheckUnlockButtonAvailability()
    {
        _multiplier = PlayerPrefs.GetInt("LastMultiplier", 1);
        
        _unlockButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("$" + baseUnlockCost * _multiplier);
        
        if (StorageManager.GetTotalScore() >= baseUnlockCost * _multiplier)
        {
            EnableButton(_unlockButton.GetComponent<Button>());
        }
        else
        {
            DisableButton(_unlockButton.GetComponent<Button>());
        }
    }

    private void DisableButton(Button button)
    {
        button.interactable = false;
        button.image.DOFade(0.5f, 0.1f);
        _unlockButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0.3f, 0.1f);
    }

    private void EnableButton(Button button)
    {
        button.interactable = true;
        button.image.DOFade(1f, 0.1f);
        _unlockButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1f, 0.1f);
    }

    public void OnUnlockButtonClick()
    {
        StorageManager.SetTotalScore(StorageManager.GetTotalScore() - (baseUnlockCost * _multiplier));
        UiManager.Instance.totalScoreText.SetText("$" + StorageManager.GetTotalScore());

        PlayerPrefs.SetInt("LastMultiplier", _multiplier + 1);

        List<int> randomHandIds = new List<int>();

        foreach (GameObject card in handCards)
        {
            if (card.GetComponent<HandCard>().unlockStatus == 0)
            {
                randomHandIds.Add(card.GetComponent<HandCard>().handId);
            }

            card.GetComponent<HandCard>().selectionImage.SetActive(false);
        }
        
        DisableButton(_unlockButton.GetComponent<Button>());
        StartCoroutine(RandomizeVisualEffect(randomHandIds));
    }

    IEnumerator RandomizeVisualEffect(List<int> randomHandIds)
    {
        HandCard handCard = null;

        int i = 0;

        while (i < 10)
        {
            int randomId = Random.Range(0, randomHandIds.Count);

            int randomUnlockId = randomHandIds[randomId];

            handCard = handCards[randomUnlockId].GetComponent<HandCard>();
            handCard.selectionImage.SetActive(true);

            yield return new WaitForSeconds(0.2f);

            handCard.selectionImage.SetActive(false);
            i++;
        }

        if (handCard != null)
        {
            handCard.UnlockHandCard(handCard.handId);
        }

        if (randomHandIds.Count <= 1)
        {
            _unlockButton.SetActive(false);
        }

        CheckUnlockButtonAvailability();
    }
}
