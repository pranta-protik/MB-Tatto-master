using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(HandCardOld))]
public class HandCardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        HandCardOld handCardOld = (HandCardOld) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("requirementType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handId"));

        if (handCardOld.requirementType == HandCardOld.ERequirementType.Cash)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredCash"));
        }
        else if (handCardOld.requirementType == HandCardOld.ERequirementType.Time)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredTime"));
        }
        else if (handCardOld.requirementType == HandCardOld.ERequirementType.GamePlay)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredMatches"));
        }
        else if (handCardOld.requirementType == HandCardOld.ERequirementType.Level)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredLevelNo"));
        }

        if (handCardOld.cardType == HandCardOld.ECardType.Model)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animatorOverrideController"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animationClips"));   
        }

        serializedObject.ApplyModifiedProperties();
    }
}
