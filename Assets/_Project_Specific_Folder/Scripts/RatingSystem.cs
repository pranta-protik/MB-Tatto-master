using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RatingSystem : MonoBehaviour
{
    [SerializeField] private List<Image> _ratingStarts;
    [SerializeField] private GameObject _popUpScreen;

    private void Start()
    {
        GetComponent<Image>().DOFade(0.8f, 0.5f);
    }

    public void OnRatingStartsClick(int buttonIndex)
    {
        for (int i = 0; i < buttonIndex; i++)
        {
            _ratingStarts[i].color = Color.yellow;
        }

        if (buttonIndex < 4)
        {
            Invoke(nameof(EnablePopUp), 0.5f);
        }
        else
        {
            Invoke(nameof(GoToRatingLink), 0.5f);
        }
    }

    private void GoToRatingLink()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.manabreak.tattooevolution");
        gameObject.SetActive(false);    
        GameManager.Instance.MobileScreenTransition();
    }
    
    private void EnablePopUp()
    {
        _popUpScreen.SetActive(true);
        _popUpScreen.transform.GetChild(0).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        _popUpScreen.transform.GetChild(1).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        gameObject.SetActive(false);
        
        Invoke(nameof(DisablePopUp), 1f);
    }

    private void DisablePopUp()
    {
        _popUpScreen.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).OnComplete(() =>
        {
            _popUpScreen.SetActive(false);
            GameManager.Instance.MobileScreenTransition();
        });
    }
}
