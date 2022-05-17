using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// To prevent data loss during the Core/Geryon/GDPR merge,
    /// we need to first move some of the files so they are not deleted. 
    /// </summary>
    internal static class PreMergePostProcessor
    {
        private static readonly List<FileToMove> FilesToMove = new List<FileToMove>
        {
            // Moving DVR
            new FileToMove("Assets/Homa Games/Geryon/Runtime/DVR.cs", 
                "Assets/Homa Games/Homa Belly/Preserved/Geryon/DVR.cs"),
            
            // Moving GDPR settings
            new FileToMove("Assets/Homa Games/GDPR/Runtime/Resources/Homa Games GDPR Settings.asset", 
                "Assets/Homa Games/Homa Belly/Preserved/DataPrivacy/Resources/Homa Games DataPrivacy Settings.asset"),
        };

        private static readonly List<CodeToChange> CodePiecesToChange = new List<CodeToChange>
        {
            // Changing GDPR Settings' name/path reference
            new CodeToChange("Assets/Homa Games/GDPR/Runtime/Utilities/Constants.cs",
                @"public const string GDPR_SETTINGS_ASSET_PATH = ""Assets/Homa Games/GDPR/Runtime/Resources/Homa Games GDPR Settings.asset"";",
                "public const string GDPR_SETTINGS_ASSET_PATH = \"Assets/Homa Games/Homa Belly/Preserved/DataPrivacy/Resources/Homa Games DataPrivacy Settings.asset\";"),
            new CodeToChange("Assets/Homa Games/GDPR/Runtime/Utilities/Constants.cs",
                @"public const string SETTINGS_RESOURCE_NAME = ""Homa Games GDPR Settings"";",
                "public const string SETTINGS_RESOURCE_NAME = \"Homa Games DataPrivacy Settings\";"),
            
            // Changing DVR's path references
            new CodeToChange("Assets/Homa Games/Geryon/Runtime/Utils/Constants.cs",
                @"public const string ABSOLUTE_DVR_PATH = HOMA_GAMES_SCRIPTS_DIR_PATH \+ ""\/"" \+ DVR_FILENAME;",
                "public const string ABSOLUTE_DVR_PATH = \"Homa Games/Homa Belly/Preserved/Geryon/\" + DVR_FILENAME;"),
            new CodeToChange("Assets/Homa Games/Geryon/Runtime/Utils/Constants.cs",
                "public const string ABSOLUTE_DVR_PATH = \"Assets/Homa Games/Homa Belly/Preserved/Geryon/\" + DVR_FILENAME;",
                "public const string ABSOLUTE_DVR_PATH = \"Homa Games/Homa Belly/Preserved/Geryon/\" + DVR_FILENAME;")
        };
        
#if !HOMA_BELLY_DEV_ENV
        [InitializeOnLoadMethod]
#else
        [MenuItem("Tools/Homa Belly/Test pre-merge actions")]
#endif
        private static void SetupProject()
        {
            AssetDatabase.DisallowAutoRefresh();

            bool filesChanged = false;

            try
            {
                foreach (var fileToMove in FilesToMove)
                    filesChanged |= fileToMove.TryMove();

                foreach (var codeToChange in CodePiecesToChange)
                    filesChanged |= codeToChange.TryChangeCode();
            }
            finally
            {
                AssetDatabase.AllowAutoRefresh();
                if (filesChanged)
                    AssetDatabase.Refresh();
            }
        }

        private class FileToMove
        {
            [NotNull]
            public readonly string InitialPath;
            [NotNull]
            public readonly string DestinationPath;

            public FileToMove([NotNull] string initialPath, [NotNull] string destinationPath)
            {
                InitialPath = initialPath;
                DestinationPath = destinationPath;
            }

            public bool TryMove()
            {
                if (AssetExists(InitialPath))
                {
                    CreateIntermediateDirectoriesIfNecessary(DestinationPath);
                    return AssetDatabase.MoveAsset(InitialPath, DestinationPath) == null;
                }

                return false;
            }
            
            
            private static void CreateIntermediateDirectoriesIfNecessary(string filePath)
            {
                string parentPath = Directory.GetParent(filePath)?.ToString();
                if (!string.IsNullOrEmpty(parentPath) && !Directory.Exists(parentPath))
                {
                    Directory.CreateDirectory(parentPath);
                }
            }
        }
        
        private class CodeToChange
        {
            [NotNull]
            public readonly string FilePath;
            [NotNull]
            public readonly string InitialCodeRegex;
            [NotNull]
            public readonly string NewCode;

            public CodeToChange([NotNull] string filePath, [NotNull] string initialCodeRegex, [NotNull] string newCode)
            {
                FilePath = filePath;
                InitialCodeRegex = initialCodeRegex;
                NewCode = newCode;
            }

            public bool TryChangeCode()
            {
                if (!AssetExists(FilePath))
                    return false;

                string initialFileContent = File.ReadAllText(FilePath);
                string newFileContent = Regex.Replace(initialFileContent, InitialCodeRegex, NewCode);

                if (initialFileContent != newFileContent)
                {
                    try
                    {
                        File.WriteAllText(FilePath, newFileContent);
                        return true;
                    }
                    catch (Exception)
                    {
                        // Ignored
                    }
                }
                
                return false;
            }
        }

        private static bool AssetExists(string assetPath)
        {
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(assetPath));
        }
    }
}