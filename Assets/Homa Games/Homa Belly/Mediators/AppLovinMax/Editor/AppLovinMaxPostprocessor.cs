using System;
using System.Collections.Generic;
using System.IO;
using HomaGames.HomaBelly.Utilities;
using UnityEditor;

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// Creates the configuration json file for AppLovin MAX
    /// </summary>
    public class AppLovinMaxPostprocessor
    {
        [InitializeOnLoadMethod]
        static void Configure()
        {
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                return;
            
            HomaBellyEditorLog.Debug($"Configuring {HomaBellyAppLovinMaxConstants.ID}");
            PluginManifest pluginManifest = PluginManifest.LoadFromLocalFile();

            if (pluginManifest != null)
            {
                PackageComponent packageComponent = pluginManifest.Packages
                    .GetPackageComponent(HomaBellyAppLovinMaxConstants.ID, HomaBellyAppLovinMaxConstants.TYPE);
                if (packageComponent != null)
                {
                    Dictionary<string, string> configurationData = packageComponent.Data;

                    // Applovin Ad Review feature
                    if (configurationData != null && configurationData.ContainsKey("s_sdk_key"))
                    {
                        try
                        {
                            AppLovinSettings.Instance.SdkKey = configurationData["s_sdk_key"];
                        }
                        catch (Exception e)
                        {
                            HomaBellyEditorLog.Error($"Could not set SDK Key for Applovin Ad Review: {e.Message}");
                        }
                    }
                    
                    // Create directory if does not exist
                    string parentPath = Directory.GetParent(HomaBellyAppLovinMaxConstants.CONFIG_FILE_RESOURCES_COMPLETE_PATH)?.ToString();
                    if (!string.IsNullOrEmpty(parentPath) && !Directory.Exists(parentPath))
                    {
                        Directory.CreateDirectory(parentPath);
                    }
                    
                    // Disabling auto refresh to prevent preview cache errors (see https://forum.unity.com/threads/clearpreviewcache-error.714896/ )
                    AssetDatabase.DisallowAutoRefresh();
                    File.WriteAllText(HomaBellyAppLovinMaxConstants.CONFIG_FILE_RESOURCES_COMPLETE_PATH, Json.Serialize(configurationData));

                    EditorApplication.delayCall += delegate
                    {
                        AssetDatabase.AllowAutoRefresh();
                        AssetDatabase.Refresh();
                    };
                }
            }
        }
    }
}
