/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using XiJSON.Interfaces;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XiJSON
{
    ///------------------------------------------------------------------------
    /// <summary>Any asset of this type can be stored and restore to/from
    /// streaming assets folder.</summary>
    ///------------------------------------------------------------------------

    public class JsonObject :
        ScriptableObject,
        IJsonSerializable,
        IHashData
    {
        #region Static Members

        /// <summary>Save resources to JSON when boot.</summary>
        public static bool autoRestoreResources;
        /// <summary>Save resources to JSON when we modify it.</summary>
        public static bool autoBackupResources;

        #endregion

        #region Public Fields

        [Header("JsonObject")]

        /// <summary>Help text for this asset.</summary>
        [TextArea(3, 3)]
        public string info;

        /// <summary>Reffer to the parent resource.</summary>
        protected JsonObject parentResource;

        ///--------------------------------------------------------------------
        /// <summary>Version of this record. All resources which reffers to
        /// this asset may compare this value. The difference will invoke the
        /// update gameobject process.</summary>
        ///--------------------------------------------------------------------

        [Header("JsonObject")]
        [System.NonSerialized]
        public int version = 1;

        /// <summary>The relative path of the file.</summary>
        [NaughtyAttributes.ValidateInput("NotEmpty", "Should not be empty")]
        public string relativePath;
        private bool NotEmpty(string value) { return !string.IsNullOrEmpty(value); }

        #endregion

        #region JSON Path for serailization


        ///--------------------------------------------------------------------
        /// <summary>Get path to this resource.</summary>
        ///
        /// <param name="userName">(Optional) Name of the user.</param>
        ///
        /// <returns>The JSON path.</returns>
        ///--------------------------------------------------------------------

        public void Serialize(IArchive archive)
        {
            if (archive.IsWriting)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(this); // save assets also
#endif
                archive.Write(this);
            }
            else
            {
                archive.Read(this);
                IncrementVersion();
            }
        }

        #endregion

        #region IHashData


#if XI_JSON_USE_HASH_VALUE

        ///--------------------------------------------------------------------
        /// <summary>Keep hash data for this object. The featur used for the
        /// game protection.</summary>
        ///--------------------------------------------------------------------
        
        [HideInInspector]
        public string hashData;
#endif

        /// <summary>
        /// Get the hash data
        /// </summary>
        /// <returns></returns>
        public string GetHashData()
        {
#if XI_JSON_USE_HASH_VALUE
            return hashData;
#else
            return GetHashCode().ToString();
#endif 
        }

        /// <summary>
        /// Set hash data
        /// </summary>
        /// <param name="value"></param>
        public void SetHashData(string value)
        {
#if XI_JSON_USE_HASH_VALUE
            hashData = value;
#endif
        }

        #endregion

        /// <summary>Modification of resource by editor.</summary>
        public virtual void OnValidate()
        {
            UpdateRelativePath();
            IncrementVersionInternal();
        }

        #region Version and Sync

        /// <summary>Increment version by GUI.</summary>
        public virtual void IncrementVersion()
        {
            IncrementVersionInternal();
#if UNITY_EDITOR
            // Save resources as assets
            EditorUtility.SetDirty(this);
#endif
            if (autoBackupResources)
                JsonWrite();
        }

        /// <summary>Increment version internal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void IncrementVersionInternal()
        {
            version++;
            if (parentResource != null)
            {
                if (parentResource == this)
                    throw new System.Exception("The parent never can't reffer to this object");
                parentResource.IncrementVersionInternal();
            }
        }

        #endregion
        #region Comparison

        /// <summary>A reverse comparer by name.</summary>
        public class ReverseComparerByName : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(System.Object x, System.Object y)
            {
                return (new CaseInsensitiveComparer()).Compare((y as JsonObject).name, (x as JsonObject).name);
            }
        }

        /// <summary>A forward comparer by name.</summary>
        public class ForwardComparerByName : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters 
            public int Compare(System.Object x, System.Object y)
            {
                return (new CaseInsensitiveComparer()).Compare((y as JsonObject).name, (x as JsonObject).name);
            }
        }
        #endregion

        #region Tools And Buttons

        /// <summary>Compute and update JSON file path.</summary>
        [Button()]
        public void UpdateRelativePath()
        {
#if UNITY_EDITOR
            relativePath = JsonPathTools.GetJsonFilePath(this, true);
#endif
        }

        /// <summary>Writing JSON to clipboard.</summary>
        [Button("Copy JSON")]
        public virtual void JsonCopy()
        {
#if UNITY_EDITOR
            EditorGUIUtility.systemCopyBuffer = JsonTools.ToJson(this, true);
#endif
        }

        ///--------------------------------------------------------------------
        /// <summary>Reading from JSON file this data chunk.</summary>
        ///
        /// <returns>True if it succeeds, false if it fails.</returns>
        ///--------------------------------------------------------------------

        [Button("Import")]
        private void JsonRead()
        {
            Serialize(new JsonArchive(EArchiveMode.Reading, this));
        }

        /// <summary>Writing to JSON file this data chunk.</summary>
        [Button("Export")]
        private void JsonWrite()
        {
            Serialize(new JsonArchive(EArchiveMode.Writing, this));
        }

        #endregion

#if XI_DEBUG_MENU
        ///--------------------------------------------------------------------
        /// <summary>Define a debug menu Shift+E for modifying resources by
        /// SHIFT-E menu.</summary>
        ///
        /// <param name="parentResource">   Reffer to the parent resource.</param>
        /// <param name="parentMenu">    The parent menu.</param>
        /// <param name="order">         (Optional) The order.</param>
        ///--------------------------------------------------------------------

        public virtual void CreateMenu(JsonResource parentResource, DebugMenu parentMenu, int order = 0)
        {
            this.parentResource = parentResource;
        }
#endif
    }

}