/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using XiJSON.Interfaces;
using NaughtyAttributes;
using XiJSON.Libs;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XiJSON
{
    // Class: JsonAsset
    //
    // Any asset of this type can be stored and restore to/from streaming assets folder.

    public class JsonAsset :
        ScriptableObject,
        IJsonSerializable,
        IHashable
    {
        #region Static Members

        // Save resources to JSON when boot.
        public static bool autoRestoreResources;
        // Save resources to JSON when we modify it.
        public static bool autoBackupResources;

        #endregion

        #region Public Fields

        [Header("JsonAsset")]

        // Help text for this asset.
        [TextArea(3, 3)]
        public string info;

        // Version of this record. All resources which reffers to this asset
        // may compare this value. The difference will invoke the update
        // gameobject process.

        [Header("JsonAsset")]
        [System.NonSerialized]
        public int version = 1;

        // Last saved version.
        private int savedVersion = 1;

        // The relative path of the file.
        [NaughtyAttributes.ValidateInput("NotEmpty", "Should not be empty")]
        public string relativePath;
        private bool NotEmpty(string value) { return !string.IsNullOrEmpty(value); }

        #endregion

        #region JSON Serailization

        // Function: Serialize
        //
        // Serialize this object to the given stream.
        //
        // Param:
        // archive -  The archive.

        public bool Serialize(IArchive archive)
        {
            var path = string.Empty;
            if (string.IsNullOrEmpty(relativePath))
            {
                Debug.LogError($"JsonAsset '{name}' has blank relative path!");
                path = JsonPathTools.GetProjectFilePath($"{name}.lost.json");
            }
            else
            {
                path = JsonPathTools.GetProjectFilePath(relativePath);
            }
            if (archive.IsWriting)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(this); // save assets also
#endif
                return archive.Write(this, path);
            }
            else
            {
                if (archive.Read(this, path))
                {
                    IncrementVersion();
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region IHashable


        // < Keep hash data for this object. The featur used for the game protection.
        [HideInInspector]
        public string hashData;

        // Property: HashData
        //
        // Gets or sets information describing the hash.
        //
        // Returns: Information describing the hash.

        public string HashData
        {
            get { return hashData; }
            set { hashData = value; }
        }

        #endregion

        // Function: OnValidate
        //
        // Called when the script is loaded or a value is changed in the
        // inspector (Called in the editor only)

        public virtual void OnValidate()
        {
            UpdateRelativePath();
            IncrementVersion();
        }

        #region Version and Sync

        // Property: Version
        //
        // Gets the version.
        //
        // Returns: The version.

        public int Version => version;

        // Property: IsSaved
        //
        // Gets a value indicating whether this object is saved.
        //
        // Returns: True if this object is saved, false if not.

        public bool IsSaved => version == savedVersion;

        // Function: IncrementVersion
        //
        // Increment version.

        public virtual void IncrementVersion()
        {
            version++;
#if UNITY_EDITOR
            // Save resources as assets
            EditorUtility.SetDirty(this);
#endif
        }


        #endregion


        #region Tools And Buttons

        // Compute and update JSON file path.
        [Button()]
        public void UpdateRelativePath()
        {
#if UNITY_EDITOR
            relativePath = JsonPathTools.GetJsonFilePath(this, true);
#endif
        }

        // Writing JSON to clipboard.
        [Button("Copy JSON")]
        public virtual void JsonCopy()
        {
#if UNITY_EDITOR
            EditorGUIUtility.systemCopyBuffer = JsonTools.ToJson(this, true);
#endif
        }

        //--------------------------------------------------------------------
        // Reading from JSON file this data chunk.
        //
        // <returns>True if it succeeds, false if it fails.</returns>
        //--------------------------------------------------------------------

        [Button("Import")]
        private void JsonRead()
        {
            Serialize(new JsonArchive(EArchiveMode.Reading));
        }

        // Writing to JSON file this data chunk.
        [Button("Export")]
        private void JsonWrite()
        {
            Serialize(new JsonArchive(EArchiveMode.Writing));
        }

        #endregion

        // Function: CreateMenu
        //
        // Creates a menu.
        //
        // Param:
        // parent -     The parent asste.
        // menuPath -   The parent menu.
        // MenuTitle -  The order.

        public virtual void CreateMenu(JsonAsset parent, string menuPath, string title, string help, string afterEvent = null)
        {

        }

        #region Comparison

        // Class: ReverseComparerByName
        //
        // A reverse comparer by name.

        public class ReverseComparerByName : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(System.Object x, System.Object y)
            {
                return (new CaseInsensitiveComparer()).Compare((y as JsonAsset).name, (x as JsonAsset).name);
            }
        }

        // Class: ForwardComparerByName
        //
        // A forward comparer by name.

        public class ForwardComparerByName : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters 
            public int Compare(System.Object x, System.Object y)
            {
                return (new CaseInsensitiveComparer()).Compare((y as JsonAsset).name, (x as JsonAsset).name);
            }
        }
        #endregion
    }

}