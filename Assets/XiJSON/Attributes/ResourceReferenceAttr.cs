/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

using System;

namespace XiJSON
{
    // The man reason is to markup the resource reference for verification
    // Usage
    // [ResourceReferenceAttr]
    // public string prefab;

    ///------------------------------------------------------------------------
    /// <summary>Attribute for resource reference by the name. This class
    /// cannot be inherited.</summary>
    ///
    /// <example>[ResourceReferenceAttr] public string prefab;</example>
    ///------------------------------------------------------------------------

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ResourceReferenceAttr : Attribute
    {
    }

    ///------------------------------------------------------------------------
    /// <summary>Attribute for locator reference by the name. This class
    /// cannot be inherited.</summary>
    ///
    /// <example>[ResourceReferenceAttr] public string locator;</example>
    ///------------------------------------------------------------------------

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LocatorReferenceAttr : Attribute
    {
    }
}