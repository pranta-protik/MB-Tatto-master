using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public List<GameObject> handCards = new List<GameObject>();
    public GameObject scrollbar;
    private float _scrollPos = 0;
    private float[] _pos;
    private HandCard selectedCard;
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
        Debug.Log(selectedCard.handId);
        Debug.Log(selectedCard.requirementType);
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
                selectedCard = transform.GetChild(i).GetComponent<HandCard>();
                selectedCard.PlayRandomAnimation();
                
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

    private void CheckCardRequirementStatus()
    {
        if (selectedCard.requirementType == HandCard.ERequirementType.Cash)
        {
            
        }
    }
}
