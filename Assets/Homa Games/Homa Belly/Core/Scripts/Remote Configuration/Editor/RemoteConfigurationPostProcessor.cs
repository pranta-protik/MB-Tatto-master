using System.Collections.Generic;
using System.IO;
using UnityEditor;
using HomaGames.HomaBelly.Utilities;

namespace HomaGames.HomaBelly
{
    public class RemoteConfigurationPostProcessor
    {
        [InitializeOnLoadMethod]
        static void Configure()
        {
            // Track data for Remote Configuration
            PluginManifest pluginManifest = PluginManifest.LoadFromLocalFile();
            if (pluginManifest != null)
            {
                TrackConfiguration(pluginManifest);
            }
        }

        /// <summary>
        /// Persists the Damysus configuration obtained from the installation
        /// process. This data will be sent to Damysus API on runtime
        /// </summary>
        private static void TrackConfiguration(PluginManifest pluginManifest)
        {
            WriteTrackingData(new Dictionary<string, object>()
            {
                { "ti", pluginManifest.AppToken },
                { "mvi", pluginManifest.Packages?.ManifestVersionId },
                { "dp", GetDependenciesAsJson(pluginManifest) }
            });
        }

        /// <summary>
        /// Obtains a dictionary with all plugin manifest dependencies
        /// and their versions
        /// </summary>
        /// <param name="pluginManifest">The current plugin manifest</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetDependenciesAsJson(PluginManifest pluginManifest)
        {
            List<PackageComponent> allPackages = new List<PackageComponent>();
            allPackages.AddRange(pluginManifest.Packages.CorePackages);
            allPackages.AddRange(pluginManifest.Packages.MediationLayers);
            allPackages.AddRange(pluginManifest.Packages.AttributionPlatforms);
            allPackages.AddRange(pluginManifest.Packages.AdNetworks);
            allPackages.AddRange(pluginManifest.Packages.AnalyticsSystems);
            allPackages.AddRange(pluginManifest.Packages.Others);

            Dictionary<string, string> dependencies = new Dictionary<string, string>();
            for (int i = 0; allPackages != null && i < allPackages.Count; i++)
            {
                dependencies.Add(allPackages[i].Id, allPackages[i].Version);
            }

            return dependencies;
        }

        /// <summary>
        /// Writes the tracking data to the Resources config file
        /// </summary>
        /// <param name="trackingData"></param>
        public static void WriteTrackingData(Dictionary<string, object> trackingData)
        {
            // Create directory if does not exist
            string parentPath = Directory.GetParent(RemoteConfigurationConstants.TRACKING_FILE_COMPLETE_PATH)?.ToString();
            if (!string.IsNullOrEmpty(parentPath) && !Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }

            File.WriteAllText(RemoteConfigurationConstants.TRACKING_FILE_COMPLETE_PATH, Json.Serialize(trackingData));
            EditorApplication.delayCall += AssetDatabase.Refresh;
        }
    }
}
