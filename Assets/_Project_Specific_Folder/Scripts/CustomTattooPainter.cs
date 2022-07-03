using UnityEngine;

public class CustomTattooPainter : MonoBehaviour
{
    public void OnDoneButtonClick()
    {
        gameObject.SetActive(false);
        
        GameManager.Instance.mainCamera.gameObject.SetActive(true);
    }
}
