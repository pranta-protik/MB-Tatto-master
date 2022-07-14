using HomaGames.HomaBelly;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitShopTrigger : MonoBehaviour
{
   private const string PlayerTag = "Player";
   private bool _isAdShown = false;

   private void Start()
   {
      PlayerPrefs.SetFloat(PlayerPrefsKey.META_WORLD_START_TIME, Time.time);
      // Rewarded Videos
      // Rewarded Suggested Event
      HomaBelly.Instance.TrackDesignEvent("rewarded:" + "suggested" + ":" + PlacementName.UNLOCK_STATION);
   }

   private void Update()
   {
      if (!_isAdShown && Time.time - PlayerPrefs.GetFloat(PlayerPrefsKey.META_WORLD_START_TIME) > UpgradesManager.Instance.interstitialAdTimer)
      {
         // Check if ad is available
         if (HomaBelly.Instance.IsInterstitialAvailable() && AdManager.Instance.isInterstitialAdEnabled)
         {
            HomaBelly.Instance.ShowInterstitial("Shop Duration Ad");
         }

         _isAdShown = true;
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if(!other.CompareTag(PlayerTag))
      {
         return;
      }

      if (PlayerPrefs.GetInt("FirstShopEncounter", 1) == 1)
      {
         PlayerPrefs.SetInt("FirstShopEncounter", 0);
         
         // Check if ad is available
         if (HomaBelly.Instance.IsInterstitialAvailable() && AdManager.Instance.isInterstitialAdEnabled)
         {
            HomaBelly.Instance.ShowInterstitial("Shop Exit Ad");
         }   
      }

      string levelId = (PlayerPrefs.GetInt("current_scene_text", 0) + 1).ToString();
      
      float duration = Time.time - PlayerPrefs.GetFloat(PlayerPrefsKey.META_WORLD_START_TIME, 0);
      
      // Meta World
      // Meta Session Event
      HomaBelly.Instance.TrackDesignEvent("Meta_Session:" + levelId, duration);
      
      SceneManager.LoadSceneAsync((int) SceneIndexes.SPLASH);
   }
}
