/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using XiCore.UnityExtensions;

namespace XiJSON
{
    public class JsonPathTools : MonoBehaviour
    {
        public static string TempUserName => "temp";
        public static string UserName => Environment.UserName;

        /// <summary>
        ///     Make json path for given asset
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
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
        private const string PROJECT_PATH_FORMAT = "{0}/{1}/{2}";

        public static string GetProjectFilePath(string path, string userName = null)
        {
#if USE_PER_USER_FOLDER

            return string.Format(
                PROJECT_PATH_FORMAT,
                Application.streamingAssetsPath,
                userName ?? UserName,
                path);
#else
            return string.Format(
                PROJECT_PATH_FORMAT,
                Application.streamingAssetsPath,
                ConfigFolder,
                path);

#endif
        }

        /// <summary>
        ///     Make json path for given asset
        /// </summary>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static string GetJsonFilePath(MonoBehaviour behaviour, string userName = null)
        {
            var gameObject = behaviour.gameObject;
#if USE_PER_USER_FOLDER
            return string.Format(
                   SCENE_PATH_FORMAT,
                   Application.streamingAssetsPath,
                   userName ?? UserName,
                   gameObject.scene.name,
                   behaviour.transform.GetFullPath(),
                   behaviour.GetType().Name + ".json");
#else
            return string.Format(
                   SCENE_PATH_FORMAT,
                   Application.streamingAssetsPath,
                   ConfigFolder,
                   gameObject.scene.name,
                   behaviour.transform.GetFullPath(),
                   behaviour.GetType().Name + ".json");
#endif
        }

        private const string SCENE_PATH_FORMAT = "{0}/{1}/{2}/{3}/{4}";
        private const string DEFAULT_CONFIG_FOLDER = "config";

        private static string configFolder;
        public static string ConfigFolder
        {
            get
            {
                if (configFolder == null)
                    configFolder = System.Environment.GetEnvironmentVariable("config_folder");
                if (configFolder == null)
                    configFolder = DEFAULT_CONFIG_FOLDER;
                return configFolder;
            }
        }
    }
}