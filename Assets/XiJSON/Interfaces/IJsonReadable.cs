/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

namespace XiJSON.Interfaces
{
	public interface IJsonReadable
	{
        ///--------------------------------------------------------------------
        /// <summary>Read from JSON file.</summary>
        ///
        /// <param name="filePath">The path in source folder.</param>
        ///
        /// <returns>True if it succeeds, false if it fails.</returns>
        ///--------------------------------------------------------------------

		bool JsonRead(string filePath);
	}
}