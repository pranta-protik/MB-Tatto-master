using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectionMenu : MonoBehaviour
{
    public event EventHandler OnCardUnlock;
    public GameObject handCardPrefab;
    public List<Sprite> handCardTextures = new List<Sprite>();
    private List<GameObject> _handCards = new List<GameObject>();
    private GameObject unlockButton;
    private int _totalCards;
    private void Awake()
    {
        unlockButton = transform.GetChild(3).gameObject;
        
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
            
            _handCards.Add(handCard);
        }
    }

    public void OnUnlockButtonClick()
    {
        List<int> randomHandIds = new List<int>();

        foreach (GameObject card in _handCards)
        {
            if (card.GetComponent<HandCard>().unlockStatus == 0)
            {
                randomHandIds.Add(card.GetComponent<HandCard>().handId);
            }
        }

        int randomUnlockId = Random.Range(0, randomHandIds.Count);
        randomUnlockId = randomHandIds[randomUnlockId];

        HandCard handCard = _handCards[randomUnlockId].GetComponent<HandCard>();

        handCard.unlockStatus = 1;
        handCard.lockImage.SetActive(false);
        handCard.handImage.SetActive(true);

        if (randomHandIds.Count <= 1)
        {
            unlockButton.SetActive(false);
        }
    }
}
