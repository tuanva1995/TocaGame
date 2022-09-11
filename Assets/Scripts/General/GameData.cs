using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class GameData
{
    public int coin, gem;
    public List<string> iapPackBought = new List<string>();

    public GameData()
    {
        coin = 0;
        gem = 0;
        iapPackBought = new List<string>();
    }
}

[Serializable]
public class CountObject
{
    public string id;
    public int amount;
}