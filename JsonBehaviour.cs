/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using XiJSON.Interfaces;
using UnityEngine;
using NaughtyAttributes;

namespace XiJSON
{
    public class JsonBehaviour : MonoBehaviour, 
        IJsonSerializable,
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

        #region IJsonSerializable

        ///--------------------------------------------------------------------
        /// <summary>Serialize this object to the given stream.</summary>
        ///
        /// <param name="archive">The archive.</param>
        ///--------------------------------------------------------------------

        public void Serialize(IArchive archive)
        {
            var path = JsonPathTools.GetJsonFilePath(this);
            if (archive.IsReading)
                archive.Read(this, path);
            else
                archive.Write(this, path);
        }

 
#endregion

        public virtual void OnValidate()
        {
            
        }


#region Tools 

        [Button()] void Validate() { OnValidate(); }
        [Button()] void Import() { Serialize(new JsonArchive(EArchiveMode.Reading)); }
        [Button()] void Export() { Serialize(new JsonArchive(EArchiveMode.Writing)); }

#endregion
    }
}
