/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

using System.Collections.Generic;
using UnityEngine;

namespace XiJSON.Tools
{
    public static class UniqueNameTools
    {
        private const int MaxObjectId = 999;
        static Dictionary<string, UnityEngine.Object> allGuids = new Dictionary<string, UnityEngine.Object> ();

        ///--------------------------------------------------------------------
        /// <summary>Validates the names.</summary>
        ///
        /// <param name="objects">The objects.</param>
        /// <param name="exclude">(Optional) The exclude.</param>
        ///--------------------------------------------------------------------

        private static bool ValidateNames(UnityEngine.Object[] objects, GameObject exclude = null, bool showError = false)
        {
            var result = true;
            allGuids.Clear();
            for (var i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];
                if (obj == exclude)
                    continue;
                UnityEngine.Object existingObject;
                if (allGuids.TryGetValue(obj.name, out existingObject))
                {
                    // Object already exists rename existing object
                    if (existingObject != null)
                    {
                        result = false;
                        if (showError)
                            Debug.LogError($"The object has non unique name: '{existingObject.name}'", existingObject);
                    }
                }
                else
                {
                    allGuids[obj.name] = obj;
                }
            }
            allGuids.Clear();
            return result;
        }

        ///--------------------------------------------------------------------
        /// <summary>Makes unique name of object in the group.</summary>
        ///
        /// <param name="objects">       The objects.</param>
        /// <param name="objectToRename">The object to rename.</param>
        ///--------------------------------------------------------------------

        public static void MakeUniqueName(UnityEngine.Object[] objects, GameObject objectToRename)
        {
            allGuids.Clear();
            // Display the error if there are non unique names
            ValidateNames(objects, objectToRename, true);
            var nameOnly = GetNameWithoutId(objectToRename.name);
            // this is new object and it does not have ID
            for (var id = 1; id < MaxObjectId; id++)
            {
                UnityEngine.Object existingObject = null;
                var nameWithId = GetNameWithId(nameOnly, id);
                if (allGuids.TryGetValue(nameWithId, out existingObject))
                    continue;
                objectToRename.name = nameWithId;
                allGuids.Clear();
                return;
            }
            Debug.LogError($"Can't build unique name for object '{objectToRename.name}'", objectToRename);
            allGuids.Clear();
        }

        ///--------------------------------------------------------------------
        /// <summary>Makes name with suffix numerical id.</summary>
        ///
        /// <param name="nameWithoutId">.</param>
        /// <param name="id">           .</param>
        ///
        /// <returns>The name with identifier.</returns>
        ///--------------------------------------------------------------------

        public static string GetNameWithId(string nameWithoutId, int id)
        {
            // when ID==1 the name can be without ID suffix
            return id == 0 ? nameWithoutId : nameWithoutId + "-" + id;
        }

        ///--------------------------------------------------------------------
        /// <summary>Remove a suffix "-123456789" at end of the string.</summary>
        ///
        /// <param name="name">The object name.</param>
        ///
        /// <returns>The name without identifier.</returns>
        ///--------------------------------------------------------------------

        public static string GetNameWithoutId(string name)
        {
            var digitStartAt = -1;
            // Skip digits
            for (var i = name.Length - 1; i >= 0; i--)
            {
                var c = name[i];
                if (c >= '0' && c <= '9')
                    continue;
                digitStartAt = i + 1;
                break;
            }
            // Skip dashes            
            for (var i = digitStartAt - 1; i >= 0; i--)
            {
                var c = name[i];
                if (c == '-')
                    continue;
                return name.Substring(0, i + 1);
            }
            return null;
        }

        ///--------------------------------------------------------------------
        /// <summary>Remove a suffix "-123456789" at end of the string.</summary>
        ///
        /// <param name="name">The object name.</param>
        /// <param name="id">  [out]The suffux value.</param>
        ///
        /// <returns>The name without identifier.</returns>
        ///--------------------------------------------------------------------

        public static string GetNameWithoutId(string name, out int id)
        {
            var digitStartAt = -1;
            // Skip digits
            for (var i = name.Length - 1; i >= 0; i--)
            {
                var c = name[i];
                if (c >= '0' && c <= '9')
                    continue;
                digitStartAt = i + 1;
                break;
            }
            // convert digits to ID
            if (digitStartAt >= 0)
            {
                var digits = name.Substring(digitStartAt);
                id = int.Parse(digits);
            }
            else
            {
                id = 0;
            }
            // Skip dashes            
            for (var i = digitStartAt - 1; i >= 0; i--)
            {
                var c = name[i];
                if (c == '-')
                    continue;
                return name.Substring(0, i + 1);
            }
            return null;
        }
    }
}