/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

namespace XiJSON.Interfaces
{
    public interface IJsonWritable
    {
        ///--------------------------------------------------------------------
        /// <summary>Write to JSON file.</summary>
        ///
        /// <param name="filePath">Full pathname of the file.</param>
        ///--------------------------------------------------------------------

        void JsonWrite(string filePath);
    }
}