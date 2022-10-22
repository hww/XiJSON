/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

using UnityEditor;
using UnityEngine;

namespace XiJSON
{
    public class XiJSONMenu
    {
        // ========================================================================
        // JsonObject 
        // ========================================================================

        [MenuItem("Xi/JSON/Set JSON Path")]
        private static void UpdateRelativePath()
        {
            var objects = Resources.FindObjectsOfTypeAll<JsonObject>();
            for (var i=0; i<objects.Length; i++)
                objects[i].UpdateRelativePath();
        }

        [MenuItem("Xi/JSON/Import Resources")]
        private static void ImportData()
        {
            var objects = Resources.FindObjectsOfTypeAll<JsonObject>();
            foreach (var entry in objects)
                entry.JsonRead(entry.GetJsonPath(JsonPathTools.UserName));

#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        [MenuItem("Xi/JSON/Export Resources")]
        private static void ExportData()
        {
            var objects = Resources.FindObjectsOfTypeAll<JsonObject>();
            foreach (var entry in objects)
                entry.JsonWrite(entry.GetJsonPath(JsonPathTools.UserName));
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }


#if USE_SELECTED_RESOURCE_FEATURE
        [MenuItem("Rocket/JSON/Export Selected Resources")]
        private static void ExportSelectedData()
        {
            var objects = Selection.objects;
            foreach (var entry in objects)
                if (entry is JsonResource)
                    (entry as JsonResource).JsonWrite(entry.GetJsonPath(JsonPathTools.UserName));
        }

        [MenuItem("Rocket/JSON/Export Selected Resources")]
        private static void ExportSelectedDataTemp()
        {
            var objects = Selection.objects;
            foreach (var entry in objects)
                if (entry is JsonResource)
                    (entry as JsonResource).JsonWrite(entry.GetJsonPath(JsonPathTools.TempUserName));
        }
#endif
        // ========================================================================
        // JsonBehaviour 
        // ========================================================================

        [MenuItem("Xi/JSON/Import Scene Data")]
        private static void ImportSceneData()
        {
            var objects = Object.FindObjectsOfType<JsonBehaviour>();
            foreach (var entry in objects)
                entry.JsonRead(entry.GetJsonPath(JsonPathTools.UserName));
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        [MenuItem("Xi/JSON/Export Scene Data")]
        private static void ExportSceneData()
        {
            var objects = Object.FindObjectsOfType<JsonBehaviour>();
            foreach (var entry in objects)
                entry.JsonWrite(entry.GetJsonPath(JsonPathTools.UserName));
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        [MenuItem("Xi/JSON/Validate Selected", false, 1)]
        private static void BakeSelectedSObjects()
        {
            var objects = Selection.objects;
            foreach (var o in objects)
            {
                var go = o as GameObject;
                if (go == null) continue;
                var baseBehaviour = go.GetComponent<JsonBehaviour>();
                baseBehaviour?.OnValidate();
            }
        }

        [MenuItem("Xi/JSON/Validate All", false, 1)]
        private static void BakeAllSObjects()
        {
            var objects = Object.FindObjectsOfType<JsonBehaviour>();
            foreach (var baseBehaviour in objects) baseBehaviour?.OnValidate();
        }
    }
}