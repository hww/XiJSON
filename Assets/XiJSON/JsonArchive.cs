/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using UnityEngine;
using XiJSON.Interfaces;

namespace XiJSON
{
    public enum EArchiveMode  { Reading, Writing };

    /// <summary>A JSON archive.</summary>
    public class JsonArchive : IArchive
    {
        /// <summary>The mode.</summary>
        private EArchiveMode _mode;

        /// <summary>Full pathname of the file.</summary>
        private string _path;

        ///--------------------------------------------------------------------
        /// <summary>Gets the full pathname of the file.</summary>
        ///
        /// <value>The full pathname of the file.</value>
        ///--------------------------------------------------------------------

        public string Path => _path;

        ///--------------------------------------------------------------------
        /// <summary>Gets a value indicating whether this object is writing.</summary>
        ///
        /// <value>True if this object is writing, false if not.</value>
        ///--------------------------------------------------------------------

        public bool IsWriting => _mode == EArchiveMode.Writing;

        ///--------------------------------------------------------------------
        /// <summary>Gets a value indicating whether this object is
        /// reading.</summary>
        ///
        /// <value>True if this object is reading, false if not.</value>
        ///--------------------------------------------------------------------

        public bool IsReading => _mode == EArchiveMode.Reading;

        ///--------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        ///
        /// <param name="mode">The mode.</param>
        /// <param name="mb">  The megabytes.</param>
        ///--------------------------------------------------------------------

        public JsonArchive(EArchiveMode mode, MonoBehaviour mb)
        {
            _mode = mode;
            _path = JsonPathTools.GetJsonFilePath(mb);
        }

        ///--------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        ///
        /// <param name="mode">The mode.</param>
        /// <param name="jo">  The jo.</param>
        ///--------------------------------------------------------------------

        public JsonArchive(EArchiveMode mode, JsonObject jo)
        {
            _mode = mode;
            if (string.IsNullOrEmpty(jo.relativePath))
            {
                Debug.LogError($"JsonObject '{jo.name}' has blank relative path!");
                _path = JsonPathTools.GetProjectFilePath($"{jo.name}.lost.json");
            }
            else
            {
                _path = JsonPathTools.GetProjectFilePath(jo.relativePath);
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>Writes the given jso.</summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error
        ///                                 condition occurs.</exception>
        ///
        /// <param name="jso">The jso to write.</param>
        ///--------------------------------------------------------------------

        public void Write(object jso)
        {
            if (IsWriting)
                JsonTools.JsonWrite(jso, _path);
            else
                throw new System.Exception();
        }

        ///--------------------------------------------------------------------
        /// <summary>Reads the given jso.</summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error
        ///                                 condition occurs.</exception>
        ///
        /// <param name="jso">The jso to read.</param>
        ///--------------------------------------------------------------------

        public void Read(object jso)
        {
            if (IsReading)
                JsonTools.JsonRead(jso, _path);
            else
                throw new System.Exception();
        }
    }
 
}