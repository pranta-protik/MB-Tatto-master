using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using HomaGames.HomaBelly;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string defaultUsername;
    // Start is called before the first frame update
    private int _gameOpenCount;
    
    private void Awake()
    { 

        GameAnalytics.Initialize();
        
        if (PlayerPrefs.GetInt("Played", 0) == 0)
        {
            PlayerPrefs.SetInt("SavedTattooNo", 0);
            _gameOpenCount = 0;

            PlayerPrefs.SetInt("Played", 1);
            PlayerPrefs.SetString("Username", defaultUsername);
            
            // Session
            // Game Launched Event
            HomaBelly.Instance.TrackDesignEvent("GameLaunched");

            StartCoroutine(LoadNextScene());
        }
        else
        {
            _gameOpenCount = PlayerPrefs.GetInt("GameOpenCount");
            LoadLastScene();
        }

        _gameOpenCount += 1;
        PlayerPrefs.SetInt("GameOpenCount", _gameOpenCount);
        
        PlayerPrefs.SetFloat("StartTime", Time.time);
        
        // Session
        // Session Started Event
        if (_gameOpenCount < 100)
        {
            HomaBelly.Instance.TrackDesignEvent("Session:"+_gameOpenCount+":Started", Time.time);    
        }
    }

    private IEnumerator LoadNextScene()
    {
#if UNITY_IOS
        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus(); 
        
        while (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            
            status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            yield return null;
        }
#endif
        SceneManager.LoadSceneAsync((int) SceneIndexes.SPLASH);
        yield return null;
    }
    
    private static void LoadLastScene()
    {
        SceneManager.LoadSceneAsync((int) SceneIndexes.MAIN);
    }

    // public static void LoadLevelCount()
    // {
    //     PlayerPrefs.GetInt("current_scene");
    // }
}
