using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    private Image transitionImage;
    public GameObject HandTut;
    private void Start()
    {
        transitionImage = GetComponent<Image>();

        transitionImage.DOFade(0, 1f).OnComplete(() =>
        {
            transitionImage.gameObject.SetActive(false);
            Invoke("HideHandTut", 2);
        });
    }
    void HideHandTut()
    {
        HandTut.SetActive(false);
    }
}