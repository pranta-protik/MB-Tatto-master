using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CustomTattooPainter : MonoBehaviour
{
    [SerializeField] private Image foreGround;
    [SerializeField] private GameObject canvasQuad;

    public void WhiteScreenFadeOut()
    {
        foreGround.DOFade(0f, 1f).OnComplete(() =>
        {
            foreGround.gameObject.SetActive(false);
        });
    }

    public void OnDoneButtonClick()
    {
        gameObject.SetActive(false);
        GameManager.Instance.mainCamera.gameObject.SetActive(true);
        GameManager.Instance.SetCustomTattooOnTattooHand(canvasQuad.GetComponent<MeshRenderer>().material.mainTexture);
    }
}
