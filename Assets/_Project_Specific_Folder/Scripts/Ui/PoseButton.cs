using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PoseButton : MonoBehaviour
{
    public int buttonId;
    public int animationId;
    [FormerlySerializedAs("watchAdRequired")] public bool isWatchAdRequired;
    public Sprite normalIcon;
    public Sprite watchAdIcon;
    
    [HideInInspector] public Image buttonImage;

    private void Start()
    {
        buttonImage = transform.GetComponent<Image>();
        
        if (isWatchAdRequired)
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
