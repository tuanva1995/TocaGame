using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConfigController : MonoBehaviour
{


}

[Serializable]
public class ItemData
{
    public string id;
    public string name;
    public int amount = 5;
}
