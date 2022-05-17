using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HomaGames.HomaBelly {
    
    public class MissingExportActivityList : ScriptableObject {
        
        public List<MissingExportActivity> Value;
        
        public static implicit  operator List<MissingExportActivity>(MissingExportActivityList list) {
            return list.Value;
        }

        public static MissingExportActivityList GetInstance() {
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(MissingExportActivityList));
            string assetPath = AssetDatabase.GUIDToAssetPath(guids.First());
            return AssetDatabase.LoadAssetAtPath<MissingExportActivityList>(assetPath);
        }
    }

    [Serializable]
    public class MissingExportActivity {
        public string ActivityName;
        public bool ExportValueToAdd;
        
        [TextArea(3, 5)]
        [SerializeField]
        // ReSharper disable once NotAccessedField.Local
        private string Notes;
    }

}
