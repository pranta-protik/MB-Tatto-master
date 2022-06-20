using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPrefabManager
{
    private static LevelPrefabManager instance;
    public static LevelPrefabManager Instance => instance ??= new LevelPrefabManager();
    private readonly List<string> levelIds = new List<string>();

    private string GetCurrentLevelId()
    {
        for (int i = 0; i < GameManager.Instance.totalLevelNo; i++)
        {
            levelIds.Add((101 + i).ToString());
        }
            
        int levelNo = PlayerPrefs.GetInt("current_scene", 0);

        return levelNo > levelIds.Count - 1 ? levelIds[Random.Range(0, levelIds.Count)] : levelIds[levelNo];
    }

    public GameObject GetCurrentLevelPrefab()
    {
        List<GameObject> levelPrefabs = GameManager.Instance.levelPrefabs;
        
        string currentLevelId = GetCurrentLevelId();

        foreach (GameObject levelPrefab in levelPrefabs.Where(levelPrefab => levelPrefab.name == currentLevelId))
        {
            return levelPrefab;
        }

        return levelPrefabs[Random.Range(0, levelPrefabs.Count)];
    }
}