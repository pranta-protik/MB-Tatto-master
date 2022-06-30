using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomaGames.HomaBelly.Utilities;

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// Homa Bridge is the main connector between the public facade (HomaBelly)
    /// and all the inner behaviour of the Homa Belly library. All features
    /// and callbacks will be centralized within this class.
    /// </summary>
    public class HomaBridge : IHomaBellyBridge
    {
        #region Private properties

        private InitializationStatus initializationStatus = new InitializationStatus();
        private AnalyticsHelper analyticsHelper = new AnalyticsHelper();
        #endregion

        #region Public properties

        public bool IsInitialized
        {
            get
            {
                return initializationStatus.IsInitialized;
            }
        }

        #endregion

        public void Initialize()
        {
            RemoteConfiguration.FetchRemoteConfiguration().ContinueWith((remoteConfiguration) =>
            {
                HomaGamesLog.Debug("[Homa Belly] Initializing Homa Belly after Remote Configuration fetch");
                InitializeRemoteConfigurationDependantComponents(remoteConfiguration.Result);
            }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeRemoteConfigurationIndependentComponents();
        }

        /// <summary>
        /// Initializes all those components that can be initialized
        /// before the Remote Configuration data is fetched
        /// </summary>
        private void InitializeRemoteConfigurationIndependentComponents()
        {
            // Instantiate
            int servicesCount = HomaBridgeDependencies.InstantiateServices();
            initializationStatus.SetComponentsToInitialize(servicesCount);

            // Auto-track AdEvents
            RegisterAdEventsForAnalytics();

            // Try to auto configure analytics custom dimensions from NTesting
            // This is done before initializing to ensure all analytic events
            // properly gather the custom dimension
            AutoConfigureAnalyticsCustomDimensionsForNTesting();

            // Initialize
            InitializeMediators();
            InitializeAnalytics();
            InitializeCustomerSupport();

            // Start initialization grace period timer
            initializationStatus.StartInitializationGracePeriod();
            analyticsHelper.Start();
        }

        /// <summary>
        /// Initializes all those components that require from Remote Configuration
        /// data in order to initialize
        /// </summary>
        private void InitializeRemoteConfigurationDependantComponents(RemoteConfiguration.RemoteConfigurationSetup remoteConfigurationSetup)
        {
            CrossPromotionManager.Initialize(remoteConfigurationSetup);
            InitializeAttributions(remoteConfigurationSetup);
        }

        public void SetDebug(bool enabled)
        {

        }

        public void ValidateIntegration()
        {
            // Mediators
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.ValidateIntegration();
                }
            }

            // Attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.ValidateIntegration();
                }
            }

            // Analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.ValidateIntegration();
                }
            }
        }

        public void OnApplicationPause(bool pause)
        {
            // Analytics Helper
            analyticsHelper.OnApplicationPause(pause);

            // Mediators
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.OnApplicationPause(pause);
                }
            }

            // Attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.OnApplicationPause(pause);
                }
            }
        }

        #region IHomaBellyBridge

        public void ShowRewardedVideoAd(string placementName, string placementId = null)
        {
            DefaultAnalytics.RewardedAdTriggered(placementName);

            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.ShowRewardedVideoAd(placementId);
                }
            }
        }

        public bool IsRewardedVideoAdAvailable(string placementId = null)
        {
            bool available = false;
            if (HomaBridgeDependencies.GetMediators(out var mediators) )
            {
                foreach (IMediator mediator in mediators)
                {
                    available |= mediator.IsRewardedVideoAdAvailable(placementId);
                }
            }

            return available;
        }

        // Banners
        public void LoadBanner(BannerSize size, BannerPosition position, string placementId = null, UnityEngine.Color bannerBackgroundColor = default)
        {
            TrackAdEvent(AdAction.Request, AdType.Banner, "homagames.homabelly.default", placementId);

            if (HomaBridgeDependencies.GetMediators(out var mediators ))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.LoadBanner(size, position, placementId, bannerBackgroundColor);
                }
            }
        }

        public void ShowBanner(string placementId = null)
        {
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.ShowBanner(placementId);
                }
            }
        }

        public void HideBanner(string placementId = null)
        {
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.HideBanner(placementId);
                }
            }
        }

        public void DestroyBanner(string placementId = null)
        {
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.DestroyBanner(placementId);
                }
            }
        }

        public void ShowInterstitial(string placementName, string placementId = null)
        {
            DefaultAnalytics.InterstitialAdTriggered(placementName);
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.ShowInterstitial(placementId);
                }
            }
        }

        public bool IsInterstitialAvailable(string placementId = null)
        {
            bool available = false;
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    available |= mediator.IsInterstitialAvailable(placementId);
                }
            }

            return available;
        }

        public void SetUserIsAboveRequiredAge(bool consent)
        {
            // For mediators
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.SetUserIsAboveRequiredAge(consent);
                }
            }

            // For attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.SetUserIsAboveRequiredAge(consent);
                }
            }

            // For analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.SetUserIsAboveRequiredAge(consent);
                }
            }
        }

        public void SetTermsAndConditionsAcceptance(bool consent)
        {
            // For mediators
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.SetTermsAndConditionsAcceptance(consent);
                }
            }

            // For attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.SetTermsAndConditionsAcceptance(consent);
                }
            }

            // For analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.SetTermsAndConditionsAcceptance(consent);
                }
            }
        }

        public void SetAnalyticsTrackingConsentGranted(bool consent)
        {
            Identifiers.FetchAdvertisingIdentifier();
            
            // For mediators
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.SetAnalyticsTrackingConsentGranted(consent);
                }
            }

            // For attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.SetAnalyticsTrackingConsentGranted(consent);
                }
            }

            // For analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.SetAnalyticsTrackingConsentGranted(consent);
                }
            }
        }

        public void SetTailoredAdsConsentGranted(bool consent)
        {
            // For mediators
            if (HomaBridgeDependencies.GetMediators(out var mediators))
            {
                foreach (IMediator mediator in mediators)
                {
                    mediator.SetTailoredAdsConsentGranted(consent);
                }
            }

            // For attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.SetTailoredAdsConsentGranted(consent);
                }
            }

            // For analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.SetTailoredAdsConsentGranted(consent);
                }
            }
        }

#if UNITY_PURCHASING
        public void TrackInAppPurchaseEvent(UnityEngine.Purchasing.Product product, bool isRestored = false)
        {
            /*
                As per today we don't want to track IAP events on our attribution (Singular). Reasons are:
                - This data is highly inaccurate on Singular and our data department is not using it at all
                - IAP data is tracked through RevenueCat <> Singular which should be quite more accurate
                
                TODO: We can make this attribution track optional from Homa Lab manifest
                See: https://app.asana.com/0/0/1201351293892694/f
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.TrackInAppPurchaseEvent(product, isRestored);
                }
            }
            */

            // For analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackInAppPurchaseEvent(product, isRestored);
                }
            }
        }
#endif

        public void TrackInAppPurchaseEvent(string productId, string currencyCode, double unitPrice, string transactionId = null, string payload = null, bool isRestored = false)
        {
            // IAP events are applicable to Attributions and Analytics

            // For attributions
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.TrackInAppPurchaseEvent(productId, currencyCode, unitPrice, transactionId, payload);
                }
            }

            // For analytics
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackInAppPurchaseEvent(productId, currencyCode, unitPrice, transactionId, payload);
                }
            }
        }

        public void TrackResourceEvent(ResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackResourceEvent(flowType, currency, amount, itemType, itemId);
                }
            }
        }

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression01, int score = 0)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackProgressionEvent(progressionStatus, progression01, score);
                }
            }
        }

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression01, string progression02, int score = 0)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackProgressionEvent(progressionStatus, progression01, progression02, score);
                }
            }
        }

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score = 0)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackProgressionEvent(progressionStatus, progression01, progression02, progression03, score);
                }
            }
        }

        public void TrackErrorEvent(ErrorSeverity severity, string message)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackErrorEvent(severity, message);
                }
            }
        }

        public void TrackDesignEvent(string eventName, float eventValue = 0f)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackDesignEvent(eventName, eventValue);
                }
            }
        }

        public void TrackAdEvent(AdAction adAction, AdType adType, string adNetwork, string adPlacementId)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    analytic.TrackAdEvent(adAction, adType, adNetwork, adPlacementId);
                }
            }
        }

        public void TrackAdRevenue(AdRevenueData adRevenueData)
        {
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.TrackAdRevenue(adRevenueData);
                }
            }

            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    if (analytic is IAnalyticsAdRevenue instance)
                    {
                        instance.TrackAdRevenue(adRevenueData);
                    }
                }
            }
        }

        public void TrackAttributionEvent(string eventName, Dictionary<string, object> arguments = null)
        {
            if (HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                foreach (IAttribution attribution in attributions)
                {
                    attribution.TrackEvent(eventName, arguments);
                }
            }
        }

        public void SetCustomDimension01(string customDimension)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    if (analytic is ICustomDimensions instance)
                    {
                        instance.SetCustomDimension01(customDimension);
                    }
                }
            }
        }

        public void SetCustomDimension02(string customDimension)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    if (analytic is ICustomDimensions instance)
                    {
                        instance.SetCustomDimension02(customDimension);
                    }
                }
            }
        }

        public void SetCustomDimension03(string customDimension)
        {
            if (HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                foreach (IAnalytics analytic in analytics)
                {
                    if (analytic is ICustomDimensions instance)
                    {
                        instance.SetCustomDimension03(customDimension);
                    }
                }
            }
        }

        #endregion

        #region Private helpers

        private void RegisterAdEventsForAnalytics()
        {
            // Interstitial
            Events.onInterstitialAdShowSucceededEvent += (id) =>
            {
                analyticsHelper.OnInterstitialAdWatched(id);
            };

            // Rewarded Video
            Events.onRewardedVideoAdRewardedEvent += (reward) =>
            {
                analyticsHelper.OnRewardedVideoAdWatched(reward.getPlacementName());
            };
        }

        private void AutoConfigureAnalyticsCustomDimensionsForNTesting()
        {
            // This is required after implementing Geryon <> Analytics automatic integration
            // and assign ExternalTokens to Custom Dimensions
            string customDimension01 = ""; 
            string customDimension02 = "";
            string customDimension03 = "";

            GeryonUtils.GetNTestingExternalToken("ExternalToken0").ContinueWith((externalToken0TaskResult) =>
            {
                customDimension01 = externalToken0TaskResult.Result;
                GeryonUtils.GetNTestingExternalToken("ExternalToken1").ContinueWith((externalToken1TaskResult) =>
                {
                    customDimension02 = externalToken1TaskResult.Result;

                    GeryonUtils.GetNTestingExternalToken("ExternalToken2").ContinueWith((externalToken2TaskResult) =>
                    {
                        customDimension03 = externalToken2TaskResult.Result;

                        if (!string.IsNullOrEmpty(customDimension01))
                        {
                            HomaGamesLog.Debug(string.Format("Setting Game Analytics custom dimension 01 to: {0}", customDimension01));
                            SetCustomDimension01(customDimension01);
                        }

                        if (!string.IsNullOrEmpty(customDimension02))
                        {
                            HomaGamesLog.Debug(string.Format("Setting Game Analytics custom dimension 02 to: {0}", customDimension02));
                            SetCustomDimension02(customDimension02);
                        }

                        if (!string.IsNullOrEmpty(customDimension03))
                        {
                            HomaGamesLog.Debug(string.Format("Setting Game Analytics custom dimension 03 to: {0}", customDimension03));
                            SetCustomDimension03(customDimension03);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region Mediators

        private void InitializeMediators()
        {
            if (!HomaBridgeDependencies.GetMediators(out var mediators))
            {
                HomaGamesLog.Warning($"[Homa Belly] No mediators found in this project.");
                return;
            }
            
            foreach (IMediator mediator in mediators)
            {
                try
                {
                    // For Homa Belly v1.2.0+
                    if (mediator is IMediatorWithInitializationCallback instance)
                    {
                        instance.Initialize(initializationStatus.OnInnerComponentInitialized);
                    }
                    else
                    {
                        // For Homa Belly prior 1.2.0
                        mediator.Initialize();
                    }

                    mediator.RegisterEvents();
                }
                catch (Exception e)
                {
                    HomaGamesLog.Warning($"[Homa Belly] Exception initializing {mediator}: {e.Message}");
                }
            }
        }

#endregion

#region Attributions

        private void InitializeAttributions(RemoteConfiguration.RemoteConfigurationSetup remoteConfigurationSetup)
        {
            if (!HomaBridgeDependencies.GetAttributions(out var attributions))
            {
                HomaGamesLog.Warning($"[Homa Belly] No attribution services found in this project.");
                return;
            }

            if (remoteConfigurationSetup?.AttributionConfigurationModel?.DisableSingular == true)
            {
                attributions.RemoveAll(attribution => attribution.GetType().FullName?.Contains("Singular") == true);
            }
            
            // If Geryon Scope and Variant IDs are found, report it to all Attribution
            string scopeId = "";
            string variantId = "";
            GeryonUtils.GetNTestingScopeId().ContinueWith((scopeIdTaskResult) =>
            {
                scopeId = scopeIdTaskResult.Result;
                GeryonUtils.GetNTestingVariantId().ContinueWith((variantIdTaskResult) =>
                {
                    variantId = variantIdTaskResult.Result;

                    foreach (IAttribution attribution in attributions)
                    {
                        try
                        {
                            // For Homa Belly v1.2.0+
                            if (attribution is IAttributionWithInitializationCallback instance)
                            {
                                instance.Initialize(scopeId + variantId, initializationStatus.OnInnerComponentInitialized);
                            }
                            else
                            {
                                // For Homa Belly prior 1.2.0
                                attribution.Initialize(scopeId + variantId);
                            }
                        }
                        catch (Exception e)
                        {
                            HomaGamesLog.Warning($"[Homa Belly] Exception initializing {attribution}: {e.Message}");
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

#endregion

#region Analytics

        private void InitializeAnalytics()
        {
            if (!HomaBridgeDependencies.GetAnalytics(out var analytics))
            {
                HomaGamesLog.Warning($"[Homa Belly] No analytics services found in this project.");
                return;
            }
            
            foreach (IAnalytics analytic in analytics)
            {
                try
                {
                    // For Homa Belly v1.2.0+
                    if (analytic is IAnalyticsWithInitializationCallback instance)
                    {
                        instance.Initialize(initializationStatus.OnInnerComponentInitialized);
                    }
                    else
                    {
                        // For Homa Belly prior 1.2.0
                        analytic.Initialize();
                    }
                }
                catch (Exception e)
                {
                    HomaGamesLog.Warning($"[Homa Belly] Exception initializing {analytic}: {e.Message}");
                }
            }
        }

#endregion

#region Customer Support

    private void InitializeCustomerSupport()
    {
        if (!HomaBridgeDependencies.GetCustomerSupport(out var customerSupport))
            return;
        try
        {
            customerSupport.Initialize();
        }
        catch (Exception e)
        {
            HomaGamesLog.Warning($"[Homa Belly] Exception initializing {customerSupport}: {e.Message}");
        }
    }

#endregion
    }
}
