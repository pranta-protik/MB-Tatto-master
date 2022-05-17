using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HomaGames.HomaBelly
{
    public static class DefineSymbolsUtility
    {
        public static void TrySetInitialValue(string symbol, bool value, string editorPrefKey = null)
        {
            editorPrefKey = editorPrefKey ?? $"homagames.{symbol.ToLower()}_on_first_install";
            var enabledOnce = EditorPrefs.GetInt(editorPrefKey, 0) == 1;
            
            if (!enabledOnce)
            {
                SetDefineSymbolValue(symbol, value);
                EditorPrefs.SetInt(editorPrefKey, 1);
            }
        }

        public static bool GetDefineSymbolValue(string symbol) => ContainsDefineSymbolForBothPlatforms(symbol);
        public static void SetDefineSymbolValue(string symbol, bool value) => SetDefineSymbolForAllPlatforms(symbol, value);

        private static bool ContainsDefineSymbolForBothPlatforms(string symbol)
        {
            return ContainsDefineSymbol(BuildTargetGroup.Android, symbol) &&
                   ContainsDefineSymbol(BuildTargetGroup.iOS, symbol);
        }
        
        private static bool ContainsDefineSymbol(BuildTargetGroup buildTargetGroup, string symbol)
        {
            var symbols = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';'));
            return symbols.Contains(symbol);
        }

        private static void SetDefineSymbolForAllPlatforms(string symbol, bool enabled)
        {
            SetDefineSymbol(BuildTargetGroup.Android, symbol, enabled);
            SetDefineSymbol(BuildTargetGroup.iOS, symbol, enabled);
        }
        
        private static void SetDefineSymbol(BuildTargetGroup buildTargetGroup, string symbol, bool enabled)
        {
            var symbols = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';'));
            var alreadySet = symbols.Contains(symbol);
            bool changed = false;
            if (enabled && !alreadySet)
            {
                symbols.Add(symbol);
                changed = true;
            }
            else if(!enabled && alreadySet)
            {
                symbols.Remove(symbol);
                changed = true;
            }

            if (changed)
            {
                var builtSymbols = String.Join(";", symbols);
                if (builtSymbols.StartsWith(";"))
                    builtSymbols = builtSymbols.Remove(0, 1);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup,builtSymbols);
                AssetDatabase.Refresh();
            }
        }
    }
}