using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class InitialConfiguration : IPreprocessBuildWithReport
    {

        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
#if HOMA_DEVELOPMENT
            ForceDevelopmentBuild();
            UnityEngine.Debug.LogWarning("Build started with HOMA_DEVELOPMENT Setting enabled. This build should not be pushed to the stores.");
#endif
        }
        
        [InitializeOnLoadMethod]
        static void Configure()
        {
            #region Android settings

#if HOMA_DEVELOPMENT
            ForceDevelopmentBuild();
#else
            // Gradle build system
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions) Mathf.Max((int) PlayerSettings.Android.minSdkVersion, (int) AndroidSdkVersions.AndroidApiLevel22);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Low);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
#endif

            ConfigureGradleTemplate();
            #endregion

            HomaBellyEditorLog.Debug("Project configured");
        }

        private static void ConfigureGradleTemplate()
        {
            string mainTemplateGradlePath = Application.dataPath + "/Plugins/Android/mainTemplate.gradle";
            if (File.Exists(mainTemplateGradlePath))
            {
                HomaBellyEditorLog.Debug("Gradle file detected");
            }
        }

        /// <summary>
        /// Forcing debug key, those builds cannot be pushed to the stores
        /// </summary>
        private static void ForceDevelopmentBuild()
        {
            PlayerSettings.Android.useCustomKeystore = false;
        }
    }
}