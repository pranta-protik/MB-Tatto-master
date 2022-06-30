using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public class AttributionConfigurationModel
    {
        private const string DISABLE_PREF_KEY = "homagames.attribution.disable_singular";

        private bool _disableSingular;
        public bool DisableSingular
        {
            get
            {
                try
                {
                    _disableSingular = PlayerPrefs.GetInt(DISABLE_PREF_KEY, 0) == 1;
                }
                catch
                {
                    // Ignored
                }

                return _disableSingular;
            }
            set
            {
                try
                {
                    _disableSingular = value;
                    PlayerPrefs.SetInt(DISABLE_PREF_KEY, value ? 1 : 0);
                }
                catch
                {
                    // Ignored
                }
            }
        }

        public static AttributionConfigurationModel FromRemoteConfigurationDictionary(Dictionary<string, object> remoteConfiguration)
        {
            AttributionConfigurationModel model = new AttributionConfigurationModel();

            if (remoteConfiguration == null || !remoteConfiguration.ContainsKey("o_attributions"))
            {
                return model;
            }

            // Cross Promotion Items
            Dictionary<string, object> attributionDictionary = (Dictionary<string, object>) remoteConfiguration["o_attributions"];
            if (attributionDictionary != null)
            {
                // Obtain cross promo status
                if (attributionDictionary.ContainsKey("b_disable_singular"))
                {
                    model.DisableSingular = Convert.ToBoolean(attributionDictionary["b_disable_singular"], CultureInfo.InvariantCulture);
                }
            }

            return model;
        }
    }
}