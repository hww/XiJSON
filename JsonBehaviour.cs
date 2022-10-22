/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

using XiJSON.Interfaces;
using UnityEngine;
using NaughtyAttributes;

namespace XiJSON
{
    public class JsonBehaviour : MonoBehaviour, 
        IJsonReadable, 
        IJsonWritable, 
        IJsonPathProvider,
        IHashData,
        IValidatable
    {
        #region IHashData

#if XI_JSON_USE_HASH_VALUE
        // Keep hash data for this object
        [Header("JsonAsset")]
        [HideInInspector] 
        public string hashData;

        public string GetHashData() => hashData;
        public void SetHashData(string value) => hashData = value;

#else
        public string GetHashData() => this.GetHashCode().ToString();
        public void SetHashData(string value) { }

#endif


        #endregion

        #region IJsonReadable, IJsonWritable

        ///--------------------------------------------------------------------
        /// <summary>Write to JSON file this data chunk.</summary>
        ///
        /// <param name="filePath">Full pathname of the file.</param>
        ///--------------------------------------------------------------------

        void IJsonWritable.JsonWrite(string filePath)
        {
            JsonTools.JsonWrite(this, filePath);
        }

        ///--------------------------------------------------------------------
        /// <summary>Read from JSON file this data chunk.</summary>
        ///
        /// <param name="filePath">The path in source folder.</param>
        ///
        /// <returns>True if it succeeds, false if it fails.</returns>
        ///--------------------------------------------------------------------

        bool IJsonReadable.JsonRead(string filePath)
        {
            return JsonTools.JsonRead(this, filePath);
        }

        ///--------------------------------------------------------------------
        /// <summary>Get resource path.</summary>
        ///
        /// <param name="userName">(Optional) Name of the user.</param>
        ///
        /// <returns>The JSON path.</returns>
        ///--------------------------------------------------------------------

        string IJsonPathProvider.GetJsonPath(string userName = null)
        {
            return JsonPathTools.GetJsonFilePath(this, userName);
        }
        
 
#endregion

        [Button()]
        public virtual void OnValidate()
        {
            
        }


#region Tools 

        [Button()] void Validate() { OnValidate(); }
        [Button()] void Import() { JsonRead(GetJsonPath()); }
        [Button()] void Export() { JsonWrite(GetJsonPath()); }

#endregion
    }
}
