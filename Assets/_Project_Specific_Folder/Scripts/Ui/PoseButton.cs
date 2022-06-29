using UnityEngine;
using UnityEngine.UI;

public class PoseButton : MonoBehaviour
{
    public int buttonId;
    public int animationId;
    public bool watchAdRequired;
    public Sprite normalIcon;
    public Sprite watchAdIcon;
    
    [HideInInspector] public Image buttonImage;

    private void Start()
    {
        buttonImage = transform.GetComponent<Image>();
        
        if (watchAdRequired)
        {
            if (PlayerPrefs.GetInt("PoseAdWatched" + buttonId, 0) == 0)
            {
                buttonImage.sprite = watchAdIcon;
            }
            else
            {
                buttonImage.sprite = normalIcon;
            }
        }
        else
        {
            buttonImage.sprite = normalIcon;
        }
    }
}
