using System.Diagnostics;

namespace HomaGames.HomaBelly
{
    public static class EditorAnalyticsProxy
    { 
        [Conditional("HOMA_BELLY_EDITOR_ANALYTICS_ENABLED")]
        public static void SetTokenIdentifier(string tokenIdentifier)
        {
#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
                ApiQueryModel.TokenIdentifier = tokenIdentifier;
#endif
        }
        
        [Conditional("HOMA_BELLY_EDITOR_ANALYTICS_ENABLED")]
        public static void SetManifestVersionId(string manifestVersionId)
        {
#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
                ApiQueryModel.ManifestVersionId = manifestVersionId;
#endif
        }

        [Conditional("HOMA_BELLY_EDITOR_ANALYTICS_ENABLED")]
        public static void TrackEditorAnalyticsEvent(string eventName, string eventDescription = null,
                string eventStackTrace = null, float eventValue = 0, float eventFps = 0)
        {
#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
            EditorAnalytics.TrackEditorAnalyticsEvent(eventName, eventDescription, eventStackTrace, eventValue, eventFps);
#endif
        }
    }
}
