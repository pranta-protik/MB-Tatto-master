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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("followers2"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handGroups"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerPathFollower"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tattooGun"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("wrestlingCameraTransform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_boss"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_fightingRing"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameMode"));

        if (gameManager.gameMode == GameManager.EGameMode.Test)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("specificLevelId"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
