using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GameManager gameManager = (GameManager) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalLevelNo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("levelPrefabs"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("likes"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("followers"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handGroups"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerPathFollower"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("wrestlingCameraTransform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("postProcessHandler"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tattooGuns"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goldenTattooGunMaterialColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goldenTattooGunMaterialMetallic"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goldenTattooGunMaterialSmoothness"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tattooGunSpawnEffect"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tattooEffect"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ratingDisplayLevel"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isBannerAdEnabled"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isInterstitialAdEnabled"));

        if (gameManager.isInterstitialAdEnabled)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("interstitialAdStartLevel"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameMode"));

        if (gameManager.gameMode == GameManager.EGameMode.Test)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("specificLevelId"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
