#if UNITY_EDITOR
    using UnityEditor;
#endif
using System;
using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class ManifestAdjustConfig : ScriptableObject
    {
        #region Manifest Keys

        private const string APP_TOKEN_IOS = "s_ios_app_token";
        private const string APP_TOKEN_ANDROID = "s_android_app_token";
        private const string SIGNATURE_ENABLED_IOS = "b_ios_signature_enabled";
        private const string SIGNATURE_ENABLED_ANDROID = "b_android_signature_enabled";
        /// <summary>
        /// Signature Format: (1, 551571666, 1432351753, 767562477, 477043169)
        /// </summary>
        private const string SDK_SIGNATURE_IOS = "s_ios_sdk_signature";

        private const string SDK_SIGNATURE_ANDROID = "s_android_sdk_signature";
        #endregion
        
        private static ManifestAdjustConfig m_config;

        public static ManifestAdjustConfig Instance
        {
            get
            {
                if (m_config == null)
                    m_config = Resources.Load<ManifestAdjustConfig>(HomaBellyAdjustConstants.CONFIG_FILE_PATH_IN_RESOURCES);
#if UNITY_EDITOR
                if (m_config == null)
                {
                    var newConfig = CreateInstance<ManifestAdjustConfig>();
                    newConfig.hideFlags = HideFlags.NotEditable;
                    FileUtilities.CreateIntermediateDirectoriesIfNecessary(HomaBellyAdjustConstants.CONFIG_FILE_PATH);
                    AssetDatabase.CreateAsset(newConfig, HomaBellyAdjustConstants.CONFIG_FILE_PATH);
                    AssetDatabase.SaveAssets();
                    m_config = newConfig;
                }
#endif
                return m_config;
            }
        }

        [SerializeField]
        private PlatformConfig m_IosConfig = null;
        
        [SerializeField]
        private PlatformConfig m_AndroidConfig = null;

        public bool GetTargetPlatformConfig(out PlatformConfig platformConfig)
        {
            platformConfig = null;
#if UNITY_ANDROID
            platformConfig = m_AndroidConfig;
#elif UNITY_IOS
            platformConfig = m_IosConfig;
#endif
            return platformConfig != null;
        }

        /// <summary>
        /// Use it to fill the config from the Dictionary that e can get from the manifest 
        /// </summary>
        public void FillWithValuesFromManifestDictionary(Dictionary<string,string> manifestDictionary)
        {
            if (manifestDictionary.TryGetValue(APP_TOKEN_IOS, out string iosAppToken))
            {
                m_IosConfig.AppToken = iosAppToken;
            }

            if (manifestDictionary.TryGetValue(APP_TOKEN_ANDROID, out string androidAppToken))
            {
                m_AndroidConfig.AppToken = androidAppToken;
            }
            
            ExtractSdkSignature(manifestDictionary,
                SIGNATURE_ENABLED_IOS,
                SDK_SIGNATURE_IOS,
                m_IosConfig);
            
            ExtractSdkSignature(manifestDictionary,
                SIGNATURE_ENABLED_ANDROID,
                SDK_SIGNATURE_ANDROID,
                m_AndroidConfig);

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            if (!ConfigurationForCurrentPlatformIsValid())
            {
                Debug.LogError("Adjust SDK Configuration is not valid for the current target platform.");
            }
        }

        private static void ExtractSdkSignature(Dictionary<string, string> manifestDictionary,
            string signatureEnableKey,
            string signatureKey,
            PlatformConfig platformConfig)
        {
            try
            {
                if (manifestDictionary.ContainsKey(signatureEnableKey))
                {
                    platformConfig.SDKSignatureEnabled = bool.Parse(manifestDictionary[signatureEnableKey]);
                }

                if (platformConfig.SDKSignatureEnabled
                    && manifestDictionary.TryGetValue(signatureKey, out var sdkSignature))
                {
                    var sdkSignatureArray = sdkSignature.Replace("(", "").Replace(")", "").Split(',');
                    if (sdkSignatureArray.Length == 5)
                    {
                        platformConfig.SecretId = long.Parse(sdkSignatureArray[0]);
                        platformConfig.SecretInfo1 = long.Parse(sdkSignatureArray[1]);
                        platformConfig.SecretInfo2 = long.Parse(sdkSignatureArray[2]);
                        platformConfig.SecretInfo3 = long.Parse(sdkSignatureArray[3]);
                        platformConfig.SecretInfo4 = long.Parse(sdkSignatureArray[4]);
                    }
                    else
                    {
                        Debug.LogError(
                            $"[ERROR] Adjust SDK signature is not valid. Received: {sdkSignature} Expected: (secretId,secretInfo1,secretInfo2,secretInfo3,secretInfo4,secretInfo5)");
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError("[ERROR] Error configuring Adjust with the manifest values: " + e);
            }
        }

        public bool ConfigurationForCurrentPlatformIsValid()
        {
            if (GetTargetPlatformConfig(out var config))
            {
                if (string.IsNullOrEmpty(config.AppToken))
                {
                    Debug.LogError("[ERROR] Adjust SDK: App Token is not set in the manifest file");
                    return false;
                }

            }
            else
            {
                Debug.LogError("[ERROR] Adjust SDK Configuration is not valid. Current platform not supported.");
            }
            
            if(!IsSignatureValid(config))
            {
                Debug.LogError("[ERROR] Adjust SDK: Signature is enabled but one or more of the secret info is not set in the manifest file");
                return false;
            }

            return true;
        }

        private bool IsSignatureValid(PlatformConfig config)
        {
            return !config.SDKSignatureEnabled ||
                   config.SDKSignatureEnabled 
                   && config.SecretId > 0 
                   && config.SecretInfo1 > 0 
                   && config.SecretInfo2 > 0 
                   && config.SecretInfo3 > 0 
                   && config.SecretInfo4 > 0;
        }
        
        [Serializable]
        public class PlatformConfig
        {
            [SerializeField]
            private string m_appToken = null;

            // https://help.adjust.com/en/article/sdk-signature
            [SerializeField]
            private bool m_sdkSignatureEnabled = false;
        
            [SerializeField]
            private long m_secretId = -1;
        
            [SerializeField]
            private long m_secretInfo1 = -1;
        
            [SerializeField]
            private long m_secretInfo2 = -1;
        
            [SerializeField]
            private long m_secretInfo3 = -1;
        
            [SerializeField]
            private long m_secretInfo4 = -1;

            public string AppToken
            {
                get => m_appToken;
                set => m_appToken = value;
            }

            public bool SDKSignatureEnabled
            {
                get => m_sdkSignatureEnabled;
                set => m_sdkSignatureEnabled = value;
            }

            public long SecretId
            {
                get => m_secretId;
                set => m_secretId = value;
            }

            public long SecretInfo1
            {
                get => m_secretInfo1;
                set => m_secretInfo1 = value;
            }

            public long SecretInfo2
            {
                get => m_secretInfo2;
                set => m_secretInfo2 = value;
            }

            public long SecretInfo3
            {
                get => m_secretInfo3;
                set => m_secretInfo3 = value;
            }

            public long SecretInfo4
            {
                get => m_secretInfo4;
                set => m_secretInfo4 = value;
            }
        }
    }
}