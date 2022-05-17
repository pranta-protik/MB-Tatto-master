using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using JetBrains.Annotations;
using UnityEditor.Android;
using UnityEngine;

namespace HomaGames.HomaBelly {
    /// <summary>
    /// The goal of this class is to add the android:exported
    /// attribute where needed in the AndroidManifest.xml to
    /// prevent install errors.
    ///
    /// These errors started occuring in Android API level 31+,
    /// because of this change: https://developer.android.com/about/versions/12/behavior-changes-12#exported
    /// </summary>
    public class AndroidExportedPostprocessing : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 0;

        private XNamespace AndroidNamespace;
        private MissingExportActivityList MissingExportActivityList;
        
        public void OnPostGenerateGradleAndroidProject(string path) {
            
            if (PlayerSettings.Android.targetSdkVersion < (AndroidSdkVersions) 31
                && PlayerSettings.Android.targetSdkVersion != AndroidSdkVersions.AndroidApiLevelAuto)
                return;

            string manifestFilePath = Path.Combine(path, "src/main/AndroidManifest.xml");
            XDocument manifest = XDocument.Load(manifestFilePath);

            XElement application = manifest.Element("manifest")?.Element("application");
            if (application == null) {
                HomaBellyEditorLog.Error("Manifest does not have `manifest` or `application` element");
                return;
            }
            
            AndroidNamespace = application.GetNamespaceOfPrefix("android");

            AddAllExportedToApplication(application);
            AddAllMergedActivities(application);
            
            manifest.Save(manifestFilePath);
        }

        /// <summary>
        /// Adds an <c>android:exported</c> attribute to all elements inside the
        /// "application" element that need it.
        /// </summary>
        /// <param name="application"></param>
        private void AddAllExportedToApplication(XElement application) {
            foreach (XElement element in application.Elements()) {
                string exportedValue = GetExportedAttributeValueFor(element);
                if (exportedValue != null) {
                    element.SetAttributeValue(AndroidNamespace + "exported", exportedValue); 
                }
            }
        }
        
        [CanBeNull]
        private string GetExportedAttributeValueFor(XElement element) {
            XName[] exportableElementNames = {"activity", "service", "receiver"};
            string[] intentActionsToExport = { "android.intent.action.MAIN" };

            if (exportableElementNames.Contains(element.Name) && element.Attribute(AndroidNamespace + "exported") == null) {
                XElement intentFilterElement =
                        element.Elements().FirstOrDefault(subEl => subEl.Name == "intent-filter");
                if (intentFilterElement != null) {

                    foreach (XElement actionElement in intentFilterElement.Elements().Where(el => el.Name == "action")) {
                        XAttribute actionNameAttribute = actionElement.Attribute(AndroidNamespace + "name");
                        if (intentActionsToExport.Contains(actionNameAttribute?.Value)) {
                            return "true";
                        }
                    }

                    return "false";
                }
            }

            return null;
        }
        
        
        /// <summary>
        /// Some activities from external libraries are missing the exported attribute.
        /// Because they will be added into the merged (final) AndroidManifest file, 
        /// this method add them to the manifest, with a <c>tools:node="merge"</c> attribute
        /// in addition to the <c>android:exported</c> attribute.
        /// </summary>
        /// <param name="application"></param>
        private void AddAllMergedActivities(XElement application) {
            XNamespace toolsNamespace = "http://schemas.android.com/tools";
            if (MissingExportActivityList == null)
                MissingExportActivityList = MissingExportActivityList.GetInstance();

            foreach (MissingExportActivity missingExportActivity in MissingExportActivityList.Value) {
                XElement activityElement = new XElement("activity");
                activityElement.SetAttributeValue(AndroidNamespace + "name", missingExportActivity.ActivityName);
                activityElement.SetAttributeValue(AndroidNamespace + "exported", missingExportActivity.ExportValueToAdd ? "true" : "false");
                activityElement.SetAttributeValue(toolsNamespace + "node", "merge");
                
                application.Add(activityElement);
            }
        }
    }
}