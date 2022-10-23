using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiJSON;

[CreateAssetMenu(menuName = "Xi/JSON/Example_01/JsonAssetTest")]
public class JsonAssetTest : JsonAsset
{
    // Start is called before the first frame update
    public int integerValue;
    public bool booleanValue;
    public float floatValue;
    public string stringValue;

    [JsonAssetReference]
    public string prefabStr;

    public GameObject prefabObj;
}
