using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CustomTattooPainter : MonoBehaviour
{
    [SerializeField] private GameObject canvasQuad;

    public void OnDoneButtonClick()
    {
        gameObject.SetActive(false);
        GameManager.Instance.mainCamera.gameObject.SetActive(true);
        GameManager.Instance.SetCustomTattooOnTattooHand(canvasQuad.GetComponent<MeshRenderer>().material.mainTexture);
    }
}
