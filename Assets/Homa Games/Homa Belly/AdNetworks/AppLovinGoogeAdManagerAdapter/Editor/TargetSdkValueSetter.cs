using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace HomaGames.HomaBelly
{
    internal class TargetSdkValueSetter : IPreprocessBuildWithReport
    {
        private const string SESSION_SDK_CHECK_KEY = "homa_belly.networks.GoogleAdManagerAndroidTargetPopup";
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID
            if (SessionState.GetBool(SESSION_SDK_CHECK_KEY, false))
                return;
        
            if (PlayerSettings.Android.targetSdkVersion >= (AndroidSdkVersions) 31
                || PlayerSettings.Android.targetSdkVersion == AndroidSdkVersions.AndroidApiLevelAuto)
                return;

            int userResponse = EditorUtility.DisplayDialogComplex("Target android version too low",
                    "For the Google Ad Manager network to work, the Android SDK target must be 31 or higher",
                    "Change to 31", "I will do it myself", "Change to latest available");

            if (userResponse == 0)
            {
                PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions) 31;
            }
            else if (userResponse == 2)
            {
                PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            }
            
            SessionState.SetBool(SESSION_SDK_CHECK_KEY, true);
#endif
        }
    }
}