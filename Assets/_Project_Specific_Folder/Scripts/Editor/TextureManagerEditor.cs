using UnityEditor;

[CustomEditor(typeof(TextureManager))]
public class TextureManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handBurntTexture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tattooGroups"));

        serializedObject.ApplyModifiedProperties();
    }
}
