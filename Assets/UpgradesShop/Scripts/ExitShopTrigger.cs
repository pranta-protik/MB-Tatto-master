using HomaGames.HomaBelly;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitShopTrigger : MonoBehaviour
{
   private const string PlayerTag = "Player";

   // public Action ExitShopAction;

   private void Start()
   {
      PlayerPrefs.SetFloat(PlayerPrefsKey.META_WORLD_START_TIME, Time.time);
   }

   private void OnTriggerEnter(Collider other)
   {
      if(!other.CompareTag(PlayerTag))
      {
         return;
      }
      
      string levelId = (PlayerPrefs.GetInt("current_scene_text", 0) + 1).ToString();

      float duration = Time.time - PlayerPrefs.GetFloat(PlayerPrefsKey.META_WORLD_START_TIME, 0);
      
      // Meta World
      // Meta Session Event
      HomaBelly.Instance.TrackDesignEvent("Meta_Session:" + levelId, duration);
      
      SceneManager.LoadSceneAsync((int) SceneIndexes.SPLASH);
      // ExitShopAction?.Invoke();
   }
}
