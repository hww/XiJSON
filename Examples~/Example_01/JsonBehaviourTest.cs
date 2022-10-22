/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */


using UnityEngine;

namespace XiJSON.Demo
{
    public class JsonBehaviourTest : JsonBehaviour
    {
        public int integerValue;
        public bool booleanValue;
        public float floatValue;
        public string stringValue;

        [ResourceReferenceAttr]
        public string prefabStr;
        public GameObject prefabObj;
    }
}
