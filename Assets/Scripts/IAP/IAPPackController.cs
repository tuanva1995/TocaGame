using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class IAPPackController : Singleton<IAPPackController>
{
    private Packs packs;
    protected override void OnRegistration()
    {
        TextAsset data = Resources.Load<TextAsset>("iap_packs");
        packs = JsonUtility.FromJson<Packs>(data.text);
    }
    public Pack GetPackById(string packId)
    {
        return packs.GetPackById(packId);
    }
}
[System.Serializable]
public class Packs
{
    public Pack[] packs;
    public Pack GetPackById(string packId)
    {
        foreach (Pack pack in packs)
            if (pack.id == packId)
                return pack;
        return null;
    }
    public float GetPackPrice(string packId)
    {
        foreach (var pack in packs)
            if (pack.id == packId)
                return pack.price;
        return 0;
    }
    public float GetPackLifeTime(string packId)
    {
        foreach (var pack in packs)
            if (pack.id == packId)
                return pack.lifeTime;
        return 0;
    }
}
[System.Serializable]
public class Pack
{
    public string id;
    public float price;
    public float lifeTime;//total live time in second
    public int gem, coin;
    public CountObject[] items = new CountObject[0];
}
