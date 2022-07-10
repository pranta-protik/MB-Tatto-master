using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitShopTrigger : MonoBehaviour
{
   private const string PlayerTag = "Player";

   // public Action ExitShopAction;

   private void OnTriggerEnter(Collider other)
   {
      if(other.tag != PlayerTag)
      {
         return;
      }
      SceneManager.LoadScene(UpgradesManager.Instance.GetSceneName());
      // ExitShopAction?.Invoke();
   }
}
