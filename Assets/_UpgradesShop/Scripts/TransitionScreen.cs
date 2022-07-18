using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    private Image transitionImage;
    public GameObject HandTut;

    public bool StantPanel;
    private void Start()
    {
        if (!StantPanel)
        {
            transitionImage = GetComponent<Image>();

            transitionImage.DOFade(0, 1f).OnComplete(() =>
            {
                transitionImage.gameObject.SetActive(false);

            });
        }
    }
    public void HideHandTut()
    {
        HandTut.SetActive(false);
        gameObject.SetActive(false);
    }
}