/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using XiCore.UnityExtensions;

namespace XiJSON
{
    public class JsonPathTools : MonoBehaviour
    {
        // Name of the user.
        private static string _userName;

        // Property: UserName
        //
        // Gets or sets the name of the user.
        //
        // Returns: The name of the user.

        public static string UserName
        {
            get { return _userName ??= Environment.UserName; }
            set { _userName = value; }
        }

        // True to with user name.
        public static bool WithUserName;

        // Function: GetJsonFilePath
        //
        // Gets JSON file path.
        //
        // Exception:
        // Exception -  Thrown when an exception error condition occurs.
        //
        // Param:
        // resource -      The resource.
        // relativePath -  (Optional) True to relative path.
        // userName -      (Optional) Name of the user.
        //
        // Returns: The JSON file path.

        public static string GetJsonFilePath(JsonAsset resource, bool relativePath = false, string userName = null)
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(resource);
            Debug.Assert(path != null);
            if (Path.IsPathRooted(path))
                throw new Exception();
            if (relativePath)
                return path.Replace(".asset", ".json");
            else
                return GetProjectFilePath(path.Replace(".asset", ".json"), userName);
#else
            throw new System.Exception();
#endif
        }
        // (Immutable) the project path format.
        private const string PROJECT_PATH_FORMAT = "{0}/{1}/{2}";

        // Function: GetProjectFilePath
        //
        // Gets project file path.
        //
        // Param:
        // path -      Full pathname of the file.
        // userName -  (Optional) Name of the user.
        //
        // Returns: The project file path.

        public static string GetProjectFilePath(string path, string userName = null)
        {
            if (WithUserName)
            {

                return string.Format(
                    PROJECT_PATH_FORMAT,
                    Application.streamingAssetsPath,
                    userName ?? UserName,
                    path);
            }
            else
            {
                return string.Format(
                    PROJECT_PATH_FORMAT,
                    Application.streamingAssetsPath,
                    ConfigFolder,
                    path);
            }

        }

        // Function: GetJsonFilePath
        //
        // Gets JSON file path.
        //
        // Param:
        // behaviour -  The behaviour.
        // userName -   (Optional) Name of the user.
        //
        // Returns: The JSON file path.

        public static string GetJsonFilePath(MonoBehaviour behaviour, string userName = null)
        {
            var gameObject = behaviour.gameObject;
            if (WithUserName) 
            { 
                return string.Format(
                   SCENE_PATH_FORMAT,
                   Application.streamingAssetsPath,
                   userName ?? UserName,
                   gameObject.scene.name,
                   behaviour.transform.GetFullPath(),
                   behaviour.GetType().Name + ".json");
            }
            else
            {
                return string.Format(
                       SCENE_PATH_FORMAT,
                       Application.streamingAssetsPath,
                       ConfigFolder,
                       gameObject.scene.name,
                       behaviour.transform.GetFullPath(),
                       behaviour.GetType().Name + ".json");
            }
        }

        // (Immutable) the scene path format.
        private const string SCENE_PATH_FORMAT = "{0}/{1}/{2}/{3}/{4}";

        // (Immutable) the default aconfig folder.
        private const string DEFAULT_CONFIG_FOLDER = "XiJSON";

        // Pathname of the configuration folder.
        private static string configFolder;

        // Property: ConfigFolder
        //
        // Gets the pathname of the configuration folder.
        //
        // Returns: The pathname of the configuration folder.

        public static string ConfigFolder
        {
            get
            {
                if (configFolder == null)
                    configFolder = System.Environment.GetEnvironmentVariable("XiJSON_CONFIG_FOLDER");
                if (configFolder == null)
                    configFolder = DEFAULT_CONFIG_FOLDER;
                return configFolder;
            }
        }
    }
}