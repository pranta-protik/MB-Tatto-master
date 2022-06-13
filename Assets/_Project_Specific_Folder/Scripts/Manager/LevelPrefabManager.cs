using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SuperCop.Scripts
{
    public class LevelPrefabManager
    {
        private static LevelPrefabManager instance = null;
        public static LevelPrefabManager Instance => instance ??= new LevelPrefabManager();

        private List<string> levelIdList = new List<string>();

        private string GetCurrentLevelId()
        {
            for (int i = 0; i < 50; i++)
            {
                levelIdList.Add((101 + i).ToString());
            }
            
            int levelNo = PlayerPrefs.GetInt("current_scene", 0);

            return levelNo > levelIdList.Count - 1
                ? levelIdList[Random.Range(0, levelIdList.Count)]
                : levelIdList[levelNo];
        }

        public GameObject GetCurrentLevelPrefab()
        {
            List<GameObject> levelPrefabList = GameManager.Instance.LevelPrefabs;
            string currentLevelId = GetCurrentLevelId();
            foreach (GameObject levelPrefabItem in levelPrefabList.Where(levelPrefabItem =>
                         levelPrefabItem.name == currentLevelId))
            {
                return levelPrefabItem;
            }

            return levelPrefabList[Random.Range(0, levelPrefabList.Count)];
        }
    }
}