using System.Collections.Generic;
using System.IO;
using com.adjust.sdk;
using HomaGames.HomaBelly.Installer;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class AdjustPostprocessor
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            HomaBellyEditorLog.Debug($"Configuring {HomaBellyAdjustConstants.ID}");

            ConfigureAdjustManifestConfig();

            BuildPlayerHandlerWrapper.AddBuildPlayerHandlerFilter(ManifestAdjustConfigDoesntExist);
            BuildPlayerHandlerWrapper.AddBuildPlayerHandlerFilter(ValidConfiguration);
        }

        private static void ConfigureAdjustManifestConfig()
        {
            var pluginManifest = PluginManifest.LoadFromLocalFile();
            
            var packageComponent = pluginManifest?.Packages
                .GetPackageComponent(HomaBellyAdjustConstants.ID, EditorPackageType.ATTRIBUTION_PLATFORM);

            if (packageComponent == null)
            {
                return;
            }
            
            var configurationData = packageComponent.Data;
            ManifestAdjustConfig.Instance.FillWithValuesFromManifestDictionary(configurationData);
        }

        private static bool ManifestAdjustConfigDoesntExist(BuildPlayerOptions options)
        {
            if (ManifestAdjustConfig.Instance == null)
            {
                // This should never happen, on Editor time the AdjustConfig is recreated everytime InitializeOnLoad is called.
                Debug.LogError($"[ERROR] Can't find AdjustConfig scriptable in: {HomaBellyAdjustConstants.CONFIG_FILE_PATH_IN_RESOURCES}");
                return false;
            }

            return true;
        }
        
        private static bool ValidConfiguration(BuildPlayerOptions options)
        {
            ConfigureAdjustManifestConfig();
            if (!ManifestAdjustConfig.Instance.ConfigurationForCurrentPlatformIsValid())
            {
                // Log an error but don't cancel the build
                Debug.LogError($"[ERROR] Adjust configuration is not valid. Please check your configuration file: {HomaBellyAdjustConstants.CONFIG_FILE_PATH_IN_RESOURCES}");
                return false;
            }

            return true;
        }

        private class AndroidManifestModification : IPostGenerateGradleAndroidProject
        {
            public int callbackOrder { get; }
            public void OnPostGenerateGradleAndroidProject(string path)
            {
                Android12AdIdPermission(path);
                AddProGuardRules(path);
            }

            private static void Android12AdIdPermission(string path)
            {
                // Add AD ID permission if target SDK is >= Android 12
                // https://github.com/adjust/unity_sdk#add-permission-to-gather-google-advertising-id

                var appManifestPath = Path.Combine(path, "src/main/AndroidManifest.xml");

                var targetSdkVersion = PlayerSettings.Android.targetSdkVersion;
                var add = (int)targetSdkVersion >= 31 || targetSdkVersion == AndroidSdkVersions.AndroidApiLevelAuto;
                TempAndroidManifestUtilsUntilNextCoreRelease.UpdatePermissionInManifest(appManifestPath,
                    "com.google.android.gms.permission.AD_ID",
                    add);
            }
            
            private static void AddProGuardRules(string path)
            {
                string adjustProGuardRulesPath = $"{Application.dataPath}/Homa Games/Homa Belly/Attributions/Adjust/Editor/AdjustProGuardRules.txt";
                
                if (!File.Exists(adjustProGuardRulesPath))
                {
                    Debug.LogError($"[ERROR] Can't find AdjustProGuardRules.txt file in: {adjustProGuardRulesPath}");
                    return;
                }
                
                var proGuardUnityFile = Path.Combine(path, "proguard-unity.txt");
                if (!File.Exists(proGuardUnityFile))
                {
                    Debug.LogError($"[ERROR] Can't find proguard-unity.txt file in: {proGuardUnityFile}");
                    return;
                }
                
                string proGuardRules = File.ReadAllText(adjustProGuardRulesPath);
                AndroidProguardUtils.AddProguardRules(proGuardRules,proGuardUnityFile);
            }
        }
    }
}