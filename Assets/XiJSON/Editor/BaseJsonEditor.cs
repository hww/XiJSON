﻿/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

using UnityEditor;
using UnityEngine;
using XiCore.StringTools;
using XiJSON.Interfaces;
using XiJSON.Tools;

namespace VARP.JSON.Editor
{
    ///------------------------------------------------------------------------
    /// <summary>Small bar which allows Export/Import buttons.</summary>
    ///
    /// <typeparam name="T">The type of class to edit with this editor.</typeparam>
    ///------------------------------------------------------------------------

    public class BaseJsonEditor<T> : UnityEditor.Editor where T : Component
    {
        /// <summary>The head image.</summary>
        private Texture2D headImage;
        /// <summary>Collection of scripts.</summary>
        private T[] scriptsCollection;

        /// <summary>Initialize JSON editor.</summary>
        protected void InitBaseJsonGUI()
        {
            var monoObjects = targets;
            scriptsCollection = new T[monoObjects.Length];
            for (var i = 0; i < monoObjects.Length; i++) scriptsCollection[i] = monoObjects[i] as T;
            headImage = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHeadImagePath());
        }

        /// <summary>Deinitialize JSON editor.</summary>
        protected void DeinitBaseJsonGUI()
        {
            scriptsCollection = null;
        }

        /// <summary>Draw GUI of JSON editor.</summary>
        protected void DrawBaseJsonGUI()
        {
            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            GUILayout.BeginHorizontal();
            GUILayout.Label(headImage);
#if XIJSON_PER_RESOURCE_IMPORT_EXPORT
            if (GUILayout.Button("Export"))
                Export(null);

            if (GUILayout.Button("Import"))
                Import(null);

            if (GUILayout.Button("Export Temp"))
                Export(JsonPathTools.TempUserName);

            if (GUILayout.Button("Import Temp"))
                Import(JsonPathTools.TempUserName);
#endif
#if XIJSON_COPY_PATH
            if (GUILayout.Button("Copy"))
            {
                var path = string.Empty;
                var addComa = false;
                for (var i = 0; i < scriptsCollection.Length; i++)
                {
                    var entry = scriptsCollection[i];
                    if (entry is MonoBehaviour monoBehaviour)
                    {
                        if (addComa)
                            path += ", ";
                        path += ResourceReferenceTools.GetReference(monoBehaviour.gameObject);
                        addComa = true;
                    }
                }

                if (path != string.Empty)
                {
                    Debug.Log(path);
                    GUIUtility.systemCopyBuffer = path;
                }
            }
#endif
            if (GUILayout.Button("Make Unique Name"))
            {
                var allObjects = GameObject.FindObjectsOfType<T>();
                for (var i = 0; i < scriptsCollection.Length; i++)
                {
                    var entry = scriptsCollection[i];
                    UniqueNameTools.MakeUniqueName(allObjects, entry.gameObject);
                }
            }

            if (GUILayout.Button("Decamelize"))
            {
                for (var i = 0; i < scriptsCollection.Length; i++)
                {
                    var entry = scriptsCollection[i];
                    entry.name = Humanizer.Decamelize(entry.name);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUI.EndDisabledGroup();
        }

        ///--------------------------------------------------------------------
        /// <summary>Exports.</summary>
        ///
        /// <param name="userName">Name of the user.</param>
        ///--------------------------------------------------------------------

        private void Export(string userName)
        {
            for (var i = 0; i < scriptsCollection.Length; i++)
            {
                var entry = scriptsCollection[i];
                if (entry is IJsonPathProvider provider && entry is IJsonWritable writable)
                    writable.JsonWrite(provider.GetJsonPath(userName));
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        ///--------------------------------------------------------------------
        /// <summary>Imports.</summary>
        ///
        /// <param name="userName">Name of the user.</param>
        ///--------------------------------------------------------------------

        private void Import(string userName)
        {
            for (var i = 0; i<scriptsCollection.Length; i++)
            {
                var entry = scriptsCollection[i];
                if (entry is IJsonPathProvider provider && entry is IJsonReadable readable)
                    readable.JsonRead(provider.GetJsonPath(userName));
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        ///--------------------------------------------------------------------
        /// <summary>Return header image file.</summary>
        ///
        /// <returns>The head image path.</returns>
        ///--------------------------------------------------------------------

        protected virtual string GetHeadImagePath()
        {
            return "Assets/Plugins/VARP/JSON/Icons/varp_json_base.psd";
        }
    }
}