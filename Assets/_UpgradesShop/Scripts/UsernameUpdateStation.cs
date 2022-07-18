using UnityEngine;

public class UsernameUpdateStation : MonoBehaviour, IAdUpgrade
{
    [SerializeField] private GameObject usernameUpdatePanel;
    
    public void UnlockStation()
    {
        usernameUpdatePanel.SetActive(true);
    }
}