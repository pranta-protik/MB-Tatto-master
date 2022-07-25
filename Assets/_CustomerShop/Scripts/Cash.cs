using UnityEngine;

public class Cash : MonoBehaviour
{
    private const string PlayerTag = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        if (HapticsManager.Instance.IsHapticsAllowed)
        {
            Handheld.Vibrate();   
        }

        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        
        StorageManager.SetTotalScore(StorageManager.GetTotalScore() + 100);
        
        Destroy(gameObject, 1);
    }
}
