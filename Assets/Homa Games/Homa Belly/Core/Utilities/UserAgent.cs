using System;

namespace HomaGames.HomaBelly
{
    public static class UserAgent
    {
        /// <summary>
        /// Obtain the User Agent to be sent within the requests
        /// </summary>
        /// <returns></returns>
        public static string GetUserAgent()
        {
            string userAgent = "ANDROID";
#if UNITY_IOS
            userAgent = "IPHONE";
            try
            {
                if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
                {
                    userAgent = "IPAD";
                }
            }
            catch (Exception e)
            {
                HomaGamesLog.Warning($"Could not determine iOS device generation: ${e.Message}");
            }

#endif
            return userAgent;
        }
    }
}