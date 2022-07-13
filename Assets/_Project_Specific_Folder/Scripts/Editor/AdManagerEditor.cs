using UnityEditor;

[CustomEditor(typeof(AdManager))]
public class AdManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AdManager adManager = (AdManager) target;
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isBannerAdEnabled"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isInterstitialAdEnabled"));

        if (adManager.isInterstitialAdEnabled)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("interstitialAdStartLevel"));    
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("DontDestroyLoad"));
        
        serializedObject.ApplyModifiedProperties();
    }
}