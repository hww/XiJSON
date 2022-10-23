/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using XiJSON.Interfaces;
using UnityEngine;
using NaughtyAttributes;
using XiJSON.Libs;

namespace XiJSON
{
    public class JsonBehaviour : MonoBehaviour, 
        IJsonSerializable,
        IHashable,
        IValidatable
    {
        #region IHashable

        // Keep hash data for this object. The featur used for the game
        // protection.

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

        #region IJsonSerializable

        // Function: Serialize
        //
        // Serialize this object to or from given stream.
        //
        // Param:
        // archive -  The archive.

        public void Serialize(IArchive archive)
        {
            var path = JsonPathTools.GetJsonFilePath(this);
            if (archive.IsReading)
                archive.Read(this, path);
            else
                archive.Write(this, path);
        }

 
#endregion

        // Function: OnValidate
        //
        // Called when the script is loaded or a value is changed in the
        // inspector (Called in the editor only)

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
