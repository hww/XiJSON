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
        ///--------------------------------------------------------------------

        public JsonArchive(EArchiveMode mode)
        {
            _mode = mode;
 
        }

        ///--------------------------------------------------------------------
        /// <summary>Writes the given jso.</summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error
        ///                                 condition occurs.</exception>
        ///
        /// <param name="jso">The jso to write.</param>
        ///--------------------------------------------------------------------

        public void Write(object jso, string path)
        {
            if (IsWriting)
                JsonTools.JsonWrite(jso, path);
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

        public void Read(object jso, string path)
        {
            if (IsReading)
                JsonTools.JsonRead(jso, path);
            else
                throw new System.Exception();
        }
    }
 
}