/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Read lisense file */


namespace XiJSON.Demo
{
    public class JsonBehaviourTest : JsonBehaviour
    {
        public int integerValue;
        public bool booleanValue;
        public float floatValue;
        public string stringValue;

        [ResourceReferenceAttr]
        public string prefab;
    }
}
