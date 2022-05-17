using System;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// Collection of useful identifiers to be used across different modules.
    /// Public properties within this class guarantee the value at anytime. They fetched automatically
    /// on runtime and are updated when they are susceptible to change.
    /// </summary>
    public static class Identifiers
    {
        private const string ADVERTISING_ID_EMPTY = "00000000-0000-0000-0000-000000000000";
        private const string HOMA_ID_KEY = "hb_homa_id";
        
        /// <summary>
        /// iOS: Identifier for vendors.
        /// The value of the IDFV is the same for apps from the same developer running on the same device.
        /// </summary>
        public static string Idfv { get; private set; } = ADVERTISING_ID_EMPTY;
        
        /// <summary>
        /// iOS: Identifier for advertising
        /// Random device identifier assigned by Apple to a user’s device to be using for ad tracking.
        /// </summary>
        public static string Idfa { get; private set; } = ADVERTISING_ID_EMPTY;
        
        /// <summary>
        /// Android:  Identifier for advertisers that allows them to anonymously track user ad activity on Android devices
        /// </summary>
        public static string Gaid { get; private set; } = ADVERTISING_ID_EMPTY;
        
        /// <summary>
        /// Android: App Set ID. Identifier that will be common across all apps installed by a user from the same publisher.
        /// </summary>
        public static string Asid { get; private set; } = ADVERTISING_ID_EMPTY;
        
        /// <summary>
        /// Device unique identifier.
        /// https://docs.unity3d.com/ScriptReference/SystemInfo-deviceUniqueIdentifier.html
        /// </summary>
        public static string DeviceId { get; private set; } = SystemInfo.deviceUniqueIdentifier;
        
        /// <summary>
        /// Homa Games ID to identify this user. 
        /// We will generate a new Homa ID in each installation
        /// </summary>
        public static string HomaGamesId { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            HomaGamesId = PlayerPrefs.GetString(HOMA_ID_KEY, null);
            if (string.IsNullOrWhiteSpace(HomaGamesId))
            {
                HomaGamesId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(HOMA_ID_KEY,HomaGamesId);
                PlayerPrefs.Save();
            }
            
            FetchAdvertisingIdentifier();
        }

#pragma warning disable 1998
        public static async void FetchAdvertisingIdentifier()
#pragma warning restore 1998
        {
#if UNITY_EDITOR
            Idfv = "editor_idfv";
            Idfa = "editor_idfa";
            Gaid = "editor_gaid";
            Asid = "editor_asid";
#elif UNITY_IOS
            FetchAdvertisingIdentifierIos();
#elif UNITY_ANDROID
            FetchAdvertisingIdentifierAndroid();
#endif
            
        }
        
#if UNITY_IOS
        public static void FetchAdvertisingIdentifierIos()
        {
            Idfv = Device.vendorIdentifier;
            
            if (Idfa == ADVERTISING_ID_EMPTY)
            {

                try
                {
                    // This method was removed on Unity 2020+ for Android
                    Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnabled, string error) =>
                        {
                            if (!string.IsNullOrEmpty(error) && !string.IsNullOrWhiteSpace(advertisingId))
                            {
                                Idfa = advertisingId;
                            }
                        }
                    );
                }
                catch (Exception e)
                {
                    HomaGamesLog.Warning($"[Identifiers] Exception while fetching IDFA: {e.Message}");
                }
            }
        }
#endif

#if UNITY_ANDROID
        public static async void FetchAdvertisingIdentifierAndroid()
        {
            if (Gaid == ADVERTISING_ID_EMPTY)
            {
                try
                {
                    await Task.Run(delegate
                    {
                        // Attach thread
                        AndroidJNI.AttachCurrentThread();
                        
                        // Fetch GAID from native
                        var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                        var context = player.GetStatic<AndroidJavaObject>("currentActivity");
                        var advertisingIdClientJavaClass =
                            new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                        var advertisingIdInfoJavaObject =
                            advertisingIdClientJavaClass.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", context);
                        Gaid = advertisingIdInfoJavaObject.Call<string>("getId");
                        
                        // Detach thread
                        AndroidJNI.DetachCurrentThread();
                        
                    }).ContinueWith((result) =>
                    {
                        if (result.IsFaulted && result.Exception != null)
                        {
                            foreach (var e in result.Exception.InnerExceptions) {
                                HomaGamesLog.Warning($"[Identifiers] Exception while fetching GAID: {e.Message}");
                            }    
                        }
                        else
                        {
                            HomaGamesLog.Debug($"[Identifiers] GAID: {Gaid}");    
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions) {
                        HomaGamesLog.Warning($"[Identifiers] Exception while fetching GAID: {e.Message}");
                    }
                }
                catch (Exception e)
                {
                    HomaGamesLog.Warning($"[Identifiers] Exception while fetching GAID: {e.Message}");
                }
            }

            if (Asid == ADVERTISING_ID_EMPTY)
            {
                var homaAndroidUtilsClass = new AndroidJavaClass("AndroidAppSetIdUtility");
                homaAndroidUtilsClass.CallStatic("GetAppSetId", new GetAppSetIdCallback());
            }
        }
        
        private class GetAppSetIdCallback : AndroidJavaProxy
        {
            public GetAppSetIdCallback() : base("AndroidAppSetIdUtility$AppSetIdCallback") { }
            
            public void OnAppSetIdRetrieved(bool success,string appSetId, int scope)
            {
                if (success)
                {
                    Asid = appSetId;
                }
                
                Debug.Log($"Asid (App Set Id) Received: {success} {appSetId} {scope}");     
            }
        }
#endif
    }
}
