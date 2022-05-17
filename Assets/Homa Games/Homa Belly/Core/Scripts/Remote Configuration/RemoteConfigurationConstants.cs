using System;
using HomaGames.HomaBelly.Utilities;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public static class RemoteConfigurationConstants
    {
        [Obsolete("This variable will be removed in near future, please use TRACKING_FILE_COMPLETE_PATH by loading it from Resources")]
        public static string TRACKING_FILE = Application.streamingAssetsPath + "/Homa Games/Homa Belly/config.json";

        /// <summary>
        /// Use this path to work with System.IO
        /// </summary>
        public static string TRACKING_FILE_COMPLETE_PATH = Application.dataPath + "/Resources/Homa Games/Homa Belly/config.json";
        
        /// <summary>
        /// Use this path to load from resources.
        /// </summary>
        public static string TRACKING_FILE_RESOURCES_PATH = "Homa Games/Homa Belly/config";
        
        public static string API_FIRST_TIME_URL = HomaBellyConstants.API_HOST + "/appfirsttime?cv=" + HomaBellyConstants.API_VERSION
            + "&sv=" + HomaBellyConstants.PRODUCT_VERSION
            + "&av=" + Application.version
            + "&ti={0}"
            + "&ai=" + Application.identifier
            + "&ua={1}"
            + "&mvi={2}";
        public static string API_EVERY_TIME_URL = HomaBellyConstants.API_HOST + "/appeverytime?cv=" + HomaBellyConstants.API_VERSION
            + "&sv=" + HomaBellyConstants.PRODUCT_VERSION
            + "&av=" + Application.version
            + "&ti={0}"
            + "&ai=" + Application.identifier
            + "&ua={1}"
            + "&mvi={2}";
        public static string FIRST_TIME_ALREADY_REQUESTED = "homagames.homabelly.remoteconfiguration.first_time_already_requested";
    }
}