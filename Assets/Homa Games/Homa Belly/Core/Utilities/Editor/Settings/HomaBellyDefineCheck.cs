using UnityEditor;

namespace HomaGames.HomaBelly
{
    public static class HomaBellyDefineCheck
    {
        [InitializeOnLoadMethod]
        public static void Check()
        {
            DefineSymbolsUtility.SetDefineSymbolValue("HOMA_BELLY", true);
        }
    }
}

