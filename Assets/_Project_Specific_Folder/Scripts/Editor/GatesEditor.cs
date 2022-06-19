using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gates))]
public class GatesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Gates gates = (Gates) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("gateType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gateCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gateUICostString"));

        if (gates.gateType == EGateType.Expensive)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isSpecial"));
            if (!gates.isSpecial)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gateLevel"));
            }
        }
        else if (gates.gateType == EGateType.Cheap)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isLast"));
            if (gates.isLast)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gateLevel"));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
