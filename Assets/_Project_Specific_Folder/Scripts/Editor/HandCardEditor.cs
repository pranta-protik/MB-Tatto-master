using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(HandCard))]
public class HandCardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        HandCard handCard = (HandCard) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("requirementType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handId"));

        if (handCard.requirementType == HandCard.ERequirementType.Cash)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredCash"));
        }
        else if (handCard.requirementType == HandCard.ERequirementType.Time)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredTime"));
        }
        else if (handCard.requirementType == HandCard.ERequirementType.GamePlay)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredMatches"));
        }
        else if (handCard.requirementType == HandCard.ERequirementType.Level)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredLevelNo"));
        }

        if (handCard.cardType == HandCard.ECardType.Model)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animatorOverrideController"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animationClips"));   
        }

        serializedObject.ApplyModifiedProperties();
    }
}
