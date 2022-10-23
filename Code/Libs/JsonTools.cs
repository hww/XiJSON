/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;
using System.IO;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;
using XiCore.StringTools;
using XiJSON.Interfaces;

namespace XiJSON
{
    // Class: JsonTools
    //
    // A JSON tools.

    public static class JsonTools
    {
        private static readonly Regex regexp1 = new Regex("(,)(\\s*})");
        private static readonly Regex regexp2 = new Regex("{(\\s*)(,)");

        public static bool WithHashData;

        // Function: ToJson
        //
        // Converts this object to a JSON.
        //
        // Param:
        // obj -          Object to serialize.
        // prettyPrint -  True to pretty print.
        //
        // Returns: The given data converted to a string.

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

        // Function: JsonWrite
        //
        // JSON write object to file.
        //
        // Param:
        // obj -       Object to serialize.
        // filePath -  The file path.

        public static void JsonWrite([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            // write to file
            Debug.Log($"Writing JSON '{filePath}'");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var jsonText = ToJson(obj, true);
            File.WriteAllText(filePath, jsonText);
        }

        // Function: JsonRead
        //
        // JSON read.
        //
        // Param:
        // obj -       Object to read.
        // filePath -  The file path.
        //
        // Returns: True if it succeeds, false if it fails.

        public static bool JsonRead([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            Debug.Log("Reading JSON " + filePath);
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

        // Function: JsonWriteHashable
        //
        // JSON write hashable.
        //
        // Param:
        // obj -       Object to write file.
        // filePath -  The File path.

        public static void JsonWriteHashable([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            var hashDataInterface = obj as IHashable;
            Debug.Assert(hashDataInterface != null);
            // make hash data empty for generating new hash value
            hashDataInterface.HashData = string.Empty;
            // save to string
            var dataString = JsonUtility.ToJson(obj, true);
            // save to field
            hashDataInterface.HashData = HashTools.StringToSHA(dataString);
            // write to file
            Debug.Log("Writing JSON " + filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var jsonText = ToJson(obj, true);
            File.WriteAllText(filePath, jsonText);
        }

        // Function: JsonReadHashable
        //
        // JSON read hashable.
        //
        // Param:
        // obj -       The object. This cannot be null.
        // filePath -  The file path.
        //
        // Returns: True if it succeeds, false if it fails.

        public static bool JsonReadHashable([NotNull] object obj, [NotNull] string filePath)
        {
            Debug.Assert(filePath != null);
            var hashDataInterface = obj as IHashable;
            Debug.Assert(hashDataInterface != null);
            Debug.Log("Reading JSON " + filePath);
            var text = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(text, obj);
            // remember loaded SHA
            var readHashData = hashDataInterface.HashData;
            // now reset hash and calculate new SHA
            hashDataInterface.HashData = string.Empty;
            // save to string
            var dataString = ToJson(obj, true);
            if  (WithHashData)
                hashDataInterface.HashData = HashTools.StringToSHA(dataString);
            if (hashDataInterface.Equals(readHashData))
                return true;
            Debug.LogError("Corrupted data in " + filePath);
            return false;
        }

        // Function: RemoveInstanceID
        //
        // Removes the instance identifier.
        //
        // Param:
        // text -     Source text.
        // keyword -  The keyword.
        //
        // Returns: A result string.

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

        // Function: RemoveInstanceID
        //
        // Removes the instance identifier.
        //
        // Exception:
        // Exception -  Thrown when an exception error condition occurs.
        //
        // Param:
        // text -   A source text.
        // index -  Zero-based index of the.
        //
        // Returns: A string.

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

        // Function: SkipArrayAtRight
        //
        // Skip array at right.
        //
        // Exception:
        // Exception -  Thrown when an exception error condition occurs.
        //
        // Param:
        // text -   A source text.
        // index -  Zero-based index of the.
        //
        // Returns: An int.

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

        // Function: SkipCurlePairAtRight
        //
        // Skip curle pair at right.
        //
        // Param:
        // text -   A source text.
        // index -  Zero-based index of the.
        //
        // Returns: An int.

        private static int SkipCurlePairAtRight(string text, int index)
        {
            while (text[index] != '{')
                index++;
            while (text[index] != '}')
                index++;
            return index + 1;
        }

        // Function: SkipSpacesAtRight
        //
        // Skip spaces at right.
        //
        // Param:
        // text -   A source text.
        // index -  Zero-based index of the.
        //
        // Returns: An int.

        private static int SkipSpacesAtRight(string text, int index)
        {
            while (index < text.Length && 
                (text[index] == ' ' || text[index] == '\n' || text[index] == '\t' || text[index] == '\r'))
                index++;
            return index;
        }

        // Function: SkipSpacesAtLeft
        //
        // Skip spaces at left.
        //
        // Param:
        // text -   A source text.
        // index -  Zero-based index of the.
        //
        // Returns: An int.

        private static int SkipSpacesAtLeft(string text, int index)
        {
            while (index > -1 && 
                (text[index] == ' ' || text[index] == '\n' || text[index] == '\t' || text[index] == '\r'))
                index--;
            return index;
        }

        // Function: SkipNameAtLeft
        //
        // Skip name at left.
        //
        // Param:
        // text -   A source text.
        // index -  Zero-based index of the.
        //
        // Returns: An int.

        private static int SkipNameAtLeft(string text, int index)
        {
            index = SkipSpacesAtLeft(text, index);
            while (index > -1 && text[index] != ' ')
                index--;
            return index;
        }

        // Function: CreateDirectory
        //
        // Creates a directory.
        //
        // Param:
        // filePath -  Full pathname of the file.

        internal static void CreateDirectory(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
    }
}