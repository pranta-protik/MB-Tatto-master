#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class UsageReportQueryModel : EditorAnalyticsModelBase
    {
        private static readonly string PARAM_NAMESPACES = "namespaces";

        public UsageReportQueryModel(string eventName,List<string> namespaces) : base(eventName)
        {
            EventValues.Add(PARAM_NAMESPACES, namespaces);
        }
    }
}
#endif