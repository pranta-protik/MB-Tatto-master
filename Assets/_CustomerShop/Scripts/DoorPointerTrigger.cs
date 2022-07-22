using UnityEngine;

public class DoorPointerTrigger : MonoBehaviour
{
   private const string PlayerTag = "Player";
   
   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag(PlayerTag))
      {
         return;
      }
      
      if (PlayerPrefs.GetInt(PlayerPrefsKey.TUTORIAL_STEP_ONE_STATUS, 0) == 0)
      {
         PointersManager.Instance.EnableNextPointer();    
      }
   }
}
