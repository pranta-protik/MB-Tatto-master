using UnityEngine;

namespace HomaGames.HomaBelly
{
    public static class HomaBellyAppLovinMaxConstants
    {
        public const string ID = "applovin_max";
        public const PackageType TYPE = PackageType.MEDIATION_LAYER;

        /// <summary>
        /// Use this path to load from resources.
        /// </summary>
        public static string CONFIG_FILE_RESOURCES_PATH = "Homa Games/Homa Belly/Mediators/AppLovinMax/config";
        public static string CONFIG_FILE_RESOURCES_COMPLETE_PATH = Application.dataPath + "/Resources/" + CONFIG_FILE_RESOURCES_PATH + ".json";
    }
}
