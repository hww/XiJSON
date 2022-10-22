/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */

using System;
using System.IO;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;
using XiCore.StringTools;
using XiJSON.Interfaces;

namespace XiJSON
{
    public static class JsonTools
    {
        private static readonly Regex regexp1 = new Regex("(,)(\\s*})");
        private static readonly Regex regexp2 = new Regex("{(\\s*)(,)");

        /// <summary>
        ///     Convert object to JSON string. Remove not exportable data.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prettyPrint"></param>
        /// <returns></returns>
        public static string ToJson(object obj, bool prettyPrint)
        {
            //Debug.Assert(prettyPrint, "FIXME! It does not work for non pretty print");
            var jsonText = JsonUtility.ToJson(obj, true);
            jsonText = RemoveInstanceID(jsonText, "\"instanceID\"");
            jsonText = RemoveInstanceID(jsonText, "\"m_FileID\"");
            jsonText = regexp1.Replace(jsonText, "$2");
            jsonText = regexp2.Replace(jsonText, "$1");
            return jsonText;
        }

        /// <summary>
        ///     Write to JSON file this data chunk
        /// </summary>
        /// <param name="filePath"></param>
        public static void JsonWrite([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            // write to file
            Debug.Log($"Write JSON '{filePath}'");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var jsonText = ToJson(obj, true);
            File.WriteAllText(filePath, jsonText);
        }

        /// <summary>
        ///     Read from JSON file this data chunk
        /// </summary>
        /// <param name="filePath"></param>
        public static bool JsonRead([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            Debug.Log("Read JSON " + filePath);
            if (File.Exists(filePath))
            {
                var jsonText = File.ReadAllText(filePath);
                jsonText = RemoveInstanceID(jsonText, "\"instanceID\"");
                jsonText = RemoveInstanceID(jsonText, "\"m_FileID\"");
                jsonText = regexp1.Replace(jsonText, "$2");
                jsonText = regexp2.Replace(jsonText, "$1");
                JsonUtility.FromJsonOverwrite(jsonText, obj);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Write to JSON file this data chunk
        /// </summary>
        /// <param name="filePath"></param>
        public static void JsonWriteHashable([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            var hashDataInterface = obj as IHashData;
            Debug.Assert(hashDataInterface != null);
            // make hash data empty for generating new hash value
            hashDataInterface.SetHashData(string.Empty);
            // save to string
            var dataString = JsonUtility.ToJson(obj, true);
            // save to field
            hashDataInterface.SetHashData(HashTools.StringToSHA(dataString));
            // write to file
            Debug.Log("Write JSON " + filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var jsonText = ToJson(obj, true);
            File.WriteAllText(filePath, jsonText);
        }

        /// <summary>
        ///     Read from JSON file this data chunk
        /// </summary>
        /// <param name="filePath"></param>
        public static bool JsonReadHashable([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            var hashDataInterface = obj as IHashData;
            Debug.Assert(hashDataInterface != null);
            Debug.Log("Read JSON " + filePath);
            var text = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(text, obj);
            // remember loaded SHA
            var readHashData = hashDataInterface.GetHashData();
            // now reset hash and calculate new SHA
            hashDataInterface.SetHashData(string.Empty);
            // save to string
            var dataString = ToJson(obj, true);
            hashDataInterface.SetHashData(HashTools.StringToSHA(dataString));
            if (hashDataInterface.Equals(readHashData))
                return true;
            Debug.LogError("Corrupted data in " + filePath);
            return false;
        }
        /// <summary>
        /// Remove expression contains requested keyword
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>

        private static string RemoveInstanceID(string text, string keyword)
        {
            var index = text.IndexOf(keyword);
            while (index >= 0)
            {
                text = RemoveInstanceID(text, index);
                index = text.IndexOf(keyword);
            }
            return text;
        }

        private static string RemoveInstanceID(string text, int index)
        {
            // find left '{'
            var left = index;
            while (text[left] != '{')
                left--;
            left--;
            // skep spaces after left '{'
            left = SkipSpacesAtLeft(text, left);
            if (left < 0)
                throw new System.Exception(text.Substring(0,index) + "<!>" + text.Substring(index));

            // find right '}'
            var right = index;
            while (text[right] != '}')
                right++;


            // ----------------------------------------
            // analyze the result 
            // ----------------------------------------
            switch (text[left])
            {
                case ':':
                    {
                        left--;
                        left = SkipNameAtLeft(text, left);

                        var rightComa = SkipSpacesAtRight(text, right + 1);
                        if (text[rightComa] == ',')
                        {
                            right = rightComa;
                        }
                        else
                        {
                            // check if there is comman after the name
                            // then delete coma at the left or at the right
                            var leftComa = SkipSpacesAtLeft(text, left);
                            if (leftComa > 0 && text[leftComa] == ',')
                            {
                                left = leftComa;
                            }
                        }
                    }
                    break;

                case '[':
                    {
                        left--;
                        while (text[left] != ':')
                            left--;
                        left--;
                        left = SkipNameAtLeft(text, left);
                        // ---------------------------------------
                        while (text[right] != ']')
                            right++;
                        // ---------------------------------------
                        var rightComa = SkipSpacesAtRight(text, right + 1);
                        if (text[rightComa] == ',')
                        {
                            right = rightComa;
                        }
                        else
                        {
                            var leftComa = SkipSpacesAtLeft(text, left);
                            if (leftComa > 0 && text[leftComa] == ',')
                                left = leftComa;
                        }
                    }
                    break;
                default:
                    throw new System.Exception();
            }

            // remove text
            return text.Remove(left, right - left + 1);
        }
        private static int SkipArrayAtRight(string text, int index)
        {
            index = SkipSpacesAtRight(text, index);
            switch (text[index])
            {
                case ',':
                    index = SkipCurlePairAtRight(text, index);
                    return SkipArrayAtRight(text, index);
                case ']':
                    index++;
                    index = SkipSpacesAtRight(text, index);
                    if (text[index] == ',')
                        return index + 1;
                    return index;
                default:
                    throw new System.Exception();
            }
        }
        private static int SkipCurlePairAtRight(string text, int index)
        {
            while (text[index] != '{')
                index++;
            while (text[index] != '}')
                index++;
            return index + 1;
        }
        private static int SkipSpacesAtRight(string text, int index)
        {
            while (index < text.Length && 
                (text[index] == ' ' || text[index] == '\n' || text[index] == '\t' || text[index] == '\r'))
                index++;
            return index;
        }
        private static int SkipSpacesAtLeft(string text, int index)
        {
            while (index > -1 && 
                (text[index] == ' ' || text[index] == '\n' || text[index] == '\t' || text[index] == '\r'))
                index--;
            return index;
        }
        private static int SkipNameAtLeft(string text, int index)
        {
            index = SkipSpacesAtLeft(text, index);
            while (index > -1 && text[index] != ' ')
                index--;
            return index;
        }

        internal static void CreateDirectory(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
    }
}