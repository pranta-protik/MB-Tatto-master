using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonCard))]
public class ButtonCardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ButtonCard handCard = (ButtonCard) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("requirementType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handId"));

        if (handCard.requirementType == ButtonCard.BRequirementType.Cash)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredCash"));
        }
        else if (handCard.requirementType == ButtonCard.BRequirementType.Time)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredTime"));
        }
        else if (handCard.requirementType == ButtonCard.BRequirementType.GamePlay)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredMatches"));
        }
        else if (handCard.requirementType == ButtonCard.BRequirementType.Level)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredLevelNo"));
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
