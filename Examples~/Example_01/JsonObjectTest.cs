using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiJSON;

[CreateAssetMenu(menuName = "Xi/JSON/Example_01/JsonObjectTest")]
public class JsonObjectTest : JsonObject
{
    // Start is called before the first frame update
    public int integerValue;
    public bool booleanValue;
    public float floatValue;
    public string stringValue;

    [ResourceReferenceAttr]
    public string prefabStr;

    public GameObject prefabObj;
}
