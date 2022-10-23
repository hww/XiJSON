/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;

namespace XiJSON
{
    // Class: JsonBehaviourReference
    //
    // A JSON behaviour reference. This class cannot be inherited.
    // Usage:
    //  [JsonAssetReference] public string prefab;
    
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class JsonBehaviourReference : Attribute
    {
    }
}