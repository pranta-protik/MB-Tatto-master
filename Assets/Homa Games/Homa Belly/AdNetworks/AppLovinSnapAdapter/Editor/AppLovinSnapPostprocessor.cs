using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// Creates the configuration json file for AppLovin Snap Adapter
    /// </summary>
    public class AppLovinSnapPostprocessor
    {
        [InitializeOnLoadMethod]
        static void Configure()
        {
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                return;
            
            PluginManifest pluginManifest = PluginManifest.LoadFromLocalFile();

            if (pluginManifest != null)
            {
                PackageComponent packageComponent = pluginManifest.Packages
                    .GetPackageComponent(AppLovinSnapAdapterConstants.ID, AppLovinSnapAdapterConstants.TYPE);
                if (packageComponent != null)
                {
                    Dictionary<string, string> configurationData = packageComponent.Data;

                    if (configurationData != null)
                    {
                        // iOS
                        if (configurationData.ContainsKey("s_ios_app_id") && !string.IsNullOrWhiteSpace(configurationData["s_ios_app_id"]))
                        {
                            try
                            {
                                AppLovinSettings.Instance.SnapAppStoreAppId = Int32.Parse(configurationData["s_ios_app_id"], CultureInfo.InvariantCulture);
                            }
                            catch (Exception e)
                            {
                                HomaBellyEditorLog.Error($"Could not configure Snap iOS App ID: {e.Message}.");
                            }
                        }
                    }
                }
            }
        }
    }
}
