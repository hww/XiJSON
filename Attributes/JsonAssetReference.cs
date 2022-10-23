/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;

namespace XiJSON
{
    // Class: JsonAssetReference
    //
    // The man reason is to markup the resource reference for verification
    // 
    // Usage:
    //  [JsonAssetReference] public string prefab;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class JsonAssetReference : Attribute
    {
    }
}