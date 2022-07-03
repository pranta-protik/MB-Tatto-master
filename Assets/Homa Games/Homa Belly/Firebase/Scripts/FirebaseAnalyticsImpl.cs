using System;
using System.Globalization;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

namespace HomaGames.HomaBelly
{
    public class FirebaseAnalyticsImpl : IAnalyticsWithInitializationCallback, IAnalyticsAdRevenue
    {
        private FirebaseApp firebaseApp;
        private bool initialized = false;

        public void Initialize(Action onInitialized = null)
        {
#if UNITY_ANDROID
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    firebaseApp = Firebase.FirebaseApp.DefaultInstance;
                    Firebase.FirebaseApp.LogLevel = HomaGamesLog.debugEnabled ? LogLevel.Debug : LogLevel.Error;

                    // Firebase is ready to use by your app.
                    initialized = true;
                    onInitialized?.Invoke();
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
#else
            firebaseApp = Firebase.FirebaseApp.DefaultInstance;
            initialized = true;
            Firebase.FirebaseApp.LogLevel = HomaGamesLog.debugEnabled ? LogLevel.Debug : LogLevel.Error;
            onInitialized?.Invoke();
#endif
        }

        public void Initialize()
        {
            Initialize(null);
        }

        public void OnApplicationPause(bool pause)
        {
            // NO-OP
        }

        public void SetAnalyticsTrackingConsentGranted(bool consent)
        {
            if (Initialized())
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(consent);
            }            
        }

        public void SetTailoredAdsConsentGranted(bool consent)
        {
            // NO-OP
        }

        public void SetTermsAndConditionsAcceptance(bool consent)
        {
            // NO-OP
        }

        public void SetUserIsAboveRequiredAge(bool consent)
        {
            // NO-OP
        }

        public void TrackAdEvent(AdAction adAction, AdType adType, string adNetwork, string adPlacementId)
        {
            if (Initialized())
            {
                FirebaseAnalytics.LogEvent(adAction.ToString(), new Parameter[]
                {
                    new Parameter(FirebaseAnalytics.ParameterAdFormat, adType.ToString()),
                    new Parameter(FirebaseAnalytics.ParameterAdSource, adNetwork),
                    new Parameter(FirebaseAnalytics.ParameterAdUnitName, adPlacementId)
                });
            }
        }

        public void TrackDesignEvent(string eventName, float eventValue = 0)
        {
            if (Initialized())
            {
                FirebaseAnalytics.LogEvent(eventName.Replace(':', '_'), FirebaseAnalytics.EventPostScore, eventValue);
            }
        }

        public void TrackErrorEvent(ErrorSeverity severity, string message)
        {
            if (Initialized())
            {
                FirebaseAnalytics.LogEvent("ERROR", new Parameter[]
                {
                    new Parameter("severtiy", severity.ToString()),
                    new Parameter("message", message)
                });
            }
        }

        public void TrackInAppPurchaseEvent(string productId, string currencyCode, double unitPrice, string transactionId = null, string payload = null, bool isRestored = false)
        {
            if (Initialized())
            {
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase + (isRestored ? "_restored" : ""), new Parameter[]
                {
                    new Parameter(FirebaseAnalytics.ParameterCurrency, currencyCode),
                    new Parameter(FirebaseAnalytics.ParameterPrice, !isRestored ? Convert.ToDouble(unitPrice, CultureInfo.InvariantCulture) : 0),
                    new Parameter(FirebaseAnalytics.ParameterTransactionId, transactionId),
                    new Parameter(FirebaseAnalytics.ParameterItemId, productId)
                });
            }
        }

#if UNITY_PURCHASING
        public void TrackInAppPurchaseEvent(UnityEngine.Purchasing.Product product, bool isRestored = false)
        {
            TrackInAppPurchaseEvent(product.definition.id, product.metadata.isoCurrencyCode, Convert.ToDouble(product.metadata.localizedPrice, CultureInfo.InvariantCulture), product.transactionID, product.receipt, isRestored);
        }
#endif

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression01, int score = 0)
        {
            if (Initialized())
            {
                FirebaseAnalytics.LogEvent(progressionStatus.ToString(), new Parameter[]
                {
                    new Parameter("progression01", progression01),
                    new Parameter(FirebaseAnalytics.EventPostScore, score)
                });
            }
        }

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression01, string progression02, int score = 0)
        {
            if (Initialized())
            {
               FirebaseAnalytics.LogEvent(progressionStatus.ToString(), new Parameter[]
               {
                    new Parameter("progression01", progression01),
                    new Parameter("progression02", progression02),
                    new Parameter(FirebaseAnalytics.EventPostScore, score)
               });
            }
        }

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score = 0)
        {
            if (Initialized())
            {
                FirebaseAnalytics.LogEvent(progressionStatus.ToString(), new Parameter[]
                {
                    new Parameter("progression01", progression01),
                    new Parameter("progression02", progression02),
                    new Parameter("progression03", progression03),
                    new Parameter(FirebaseAnalytics.EventPostScore, score)
                });
            }
        }

        public void TrackResourceEvent(ResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
        {
            if (Initialized())
            {
                string eventName = FirebaseAnalytics.EventEarnVirtualCurrency;
                if (flowType == ResourceFlowType.Sink)
                {
                    eventName = FirebaseAnalytics.EventSpendVirtualCurrency;
                }

                FirebaseAnalytics.LogEvent(eventName.Replace(':', '_'), new Parameter[]
                {
                    new Parameter(FirebaseAnalytics.ParameterCurrency, currency),
                    new Parameter(FirebaseAnalytics.ParameterQuantity, amount),
                    new Parameter(FirebaseAnalytics.ParameterItemCategory, itemType),
                    new Parameter(FirebaseAnalytics.ParameterItemId, itemId)
                });
            }
        }

        public void ValidateIntegration()
        {
            // TODO: Check Android and iOS authentication files
        }

        #region Private methods

        private bool Initialized()
        {
            return firebaseApp != null && initialized;
        }

        #endregion
        
        #region IAnalyticsAdRevenue Implementation

        public void TrackAdRevenue(AdRevenueData adRevenueData)
        {
            if (adRevenueData == null)
            {
                return;
            }

            int parameterCount = 6;
            if (!string.IsNullOrEmpty(adRevenueData.Precision))
            {
                parameterCount = 7;
            }
            
            Parameter[] parameters = new Parameter[parameterCount];
            parameters[0] = new Parameter(FirebaseAnalytics.ParameterAdPlatform, adRevenueData.AdPlatform);
            parameters[1] =  new Parameter(FirebaseAnalytics.ParameterAdSource,adRevenueData.NetworkName);
            parameters[2] =  new Parameter(FirebaseAnalytics.ParameterAdFormat,adRevenueData.AdUnitId);
            parameters[3] =  new Parameter(FirebaseAnalytics.ParameterAdUnitName,string.IsNullOrEmpty(adRevenueData.AdUnitName) ? adRevenueData.AdUnitId : adRevenueData.AdUnitName);
            parameters[4] =  new Parameter(FirebaseAnalytics.ParameterCurrency,adRevenueData.Currency);
            // Docs require Revenue to be double and points to be used for decimal instead of commas,
            // I am assuming this is already done when AdRevenueData is created because here I am not working with strings.
            parameters[5] =  new Parameter(FirebaseAnalytics.ParameterValue,adRevenueData.Revenue);
            if (!string.IsNullOrEmpty(adRevenueData.Precision))
            {
                // According to the docs we only sent precision with Mopub, i was afraid to send it always as empty or null.
                parameters[6] =  new Parameter("precision",adRevenueData.Precision);
            }
            
            FirebaseAnalytics.LogEvent("ad_impression",parameters);
            // We need to send the same parameters again as 'custom_ad_impression' because the event ad_impression can't be marked as conversion.
            FirebaseAnalytics.LogEvent("custom_ad_impression",parameters);
        }

        #endregion
    }
}