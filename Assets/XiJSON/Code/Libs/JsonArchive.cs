/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using UnityEngine;
using XiJSON.Interfaces;

namespace XiJSON.Libs
{
    public enum EArchiveMode  { Reading, Writing };

    /// <summary>A JSON archive.</summary>
    public class JsonArchive : IArchive
    {
        /// <summary>The mode.</summary>
        private EArchiveMode _mode;

        // Property: IsWriting
        //
        // Gets a value indicating whether this object is writing.
        //
        // Returns: True if this object is writing, false if not.

        public bool IsWriting => _mode == EArchiveMode.Writing;

        // Property: IsReading
        //
        // Gets a value indicating whether this object is reading.
        //
        // Returns: True if this object is reading, false if not.

        public bool IsReading => _mode == EArchiveMode.Reading;

        // Function: JsonArchive
        //
        // Constructor.
        //
        // Param:
        // mode -  The mode.

        public JsonArchive(EArchiveMode mode)
        {
            _mode = mode;
 
        }

        // Function: Write
        //
        // Writes the object to file.
        //
        // Exception:
        // Exception -  Thrown when an exception error condition occurs.
        //
        // Param:
        // object -   The object.
        // filePath -  Full pathname of the file.

        public void Write(object @object, string filePath)
        {
            if (IsWriting)
                JsonTools.JsonWrite(@object, filePath);
            else
                throw new System.Exception();
        }

        // Function: Read
        //
        // Reads the file to the object.
        //
        // Exception:
        // Exception -  Thrown when an exception error condition occurs.
        //
        // Param:
        // object -   The object.
        // filePath -  Full pathname of the file.

        public void Read(object @object, string filePath)
        {
            if (IsReading)
                JsonTools.JsonRead(@object, filePath);
            else
                throw new System.Exception();
        }
    }
 
}