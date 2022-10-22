/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

namespace XiJSON.Interfaces
{
    public interface IJsonPathProvider
    {
        ///--------------------------------------------------------------------
        /// <summary>Gets JSON path.</summary>
        ///
        /// <param name="userName">(Optional) Name of the user.</param>
        ///
        /// <returns>The JSON path.</returns>
        ///--------------------------------------------------------------------

        string GetJsonPath(string userName = null);
    }
}