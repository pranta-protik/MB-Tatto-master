#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
#endif
using System.Globalization;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public abstract class EditorAnalyticsModelBase : EventApiQueryModel
    {
        private static string _cachedMacAddress = "";
        private static readonly string UNITY_VERSION = "unity_version";
        private static readonly string UNITY_TARGET = "unity_target";
        private static readonly string PARAM_MAC_ADDRESS = "mac_address";

        public EditorAnalyticsModelBase(string eventName)
        {
            EventCategory = "unity_editor_event";
            InstallId = EditorAnalyticsSessionInfo.userId;
            SessionId = EditorAnalyticsSessionInfo.id.ToString(CultureInfo.InvariantCulture);
            EventName = eventName;
            EventValues.Add(PARAM_MAC_ADDRESS,GetMacAddress());
            EventValues.Add(UNITY_VERSION, Application.unityVersion);
            EventValues.Add(UNITY_TARGET, EditorUserBuildSettings.activeBuildTarget);
        }

        private string GetMacAddress()
        {
            if (string.IsNullOrEmpty(_cachedMacAddress))
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Only consider Ethernet network interfaces
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        _cachedMacAddress = nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            return _cachedMacAddress;
        }
    }
}