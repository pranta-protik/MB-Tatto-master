using UnityEditor;

[CustomEditor(typeof(TextureManager))]
public class TextureManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        TextureManager textureManager = (TextureManager) target;
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("handBurntTexture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tattooClass"));

        switch (textureManager.tattooClass)
        {
            case TextureManager.ETattooClass.All:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapColorTattooIdSequences"));
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapColorTattooIdSequences"));
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapColorTattooIdSequences"));
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapColorTattooIdSequences"));
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapColorTattooIdSequences"));
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapColorTattooIdSequences"));
                break;
            
            case TextureManager.ETattooClass.Flower:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flowerCheapColorTattooIdSequences"));
                break;
            
            case TextureManager.ETattooClass.Skull:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skullCheapColorTattooIdSequences"));
                break;
            
            case TextureManager.ETattooClass.PinupGirl:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pinupGirlCheapColorTattooIdSequences"));
                break;
            
            case TextureManager.ETattooClass.Celebrity:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrityCheapColorTattooIdSequences"));
                break;
            
            case TextureManager.ETattooClass.Calligraphy:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("calligraphyCheapColorTattooIdSequences"));
                break;
            
            case TextureManager.ETattooClass.Money:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyDefaultTattoo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapBlueTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapYellowTattoos"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyExpensiveColorTattooIdSequences"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moneyCheapColorTattooIdSequences"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
