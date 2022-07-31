#if UNITY_EDITOR && UNITY_IOS

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

public class BuildPostProcessorInfo
{
    [PostProcessBuild]
    public static void OnPostBuildProcessInfo(BuildTarget target, string pathXcode)
    {
        if (target == BuildTarget.iOS)
        {
            var infoPlistPath = pathXcode + "/Info.plist";

            PlistDocument document = new PlistDocument();
            document.ReadFromString(File.ReadAllText(infoPlistPath));

            PlistElementDict elementDict = document.root;
            elementDict.SetString("NSUserTrackingUsageDescription", "The above information will be used to display ads and analyze usage.");
            
            File.WriteAllText(infoPlistPath, document.WriteToString());
        }
    }
}

#endif