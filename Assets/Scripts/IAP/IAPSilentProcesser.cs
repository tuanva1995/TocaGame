﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IAPSilentProcesser : MonoBehaviour
{
    private List<string> iapPacks = new List<string>();
    [HideInInspector] public bool canProcessIAP = false;
    private IEnumerator Start()
    {
        canProcessIAP = false;
        yield return new WaitForSeconds(1);
        while (!canProcessIAP)
            yield return null;
        foreach (var packId in iapPacks)
            ProcessPurchase(packId);
        Destroy(gameObject);
    }
    private void ProcessPurchase(string packId)
    {
        TextAsset data = null;
        data = Resources.Load<TextAsset>("iap_packs");
        string packsData = data.text;
        Packs packs = JsonUtility.FromJson<Packs>(packsData);
        Pack pack = packs.GetPackById(packId);
        if (pack != null)
        {
            DataController.Instance.Gem += pack.gem;
            DataController.Instance.SaveData();
        }
    }
    public void OnCompletePurchase(UnityEngine.Purchasing.Product product)
    {
        iapPacks.Add(product.definition.id);
    }
    public void OnFailPurchase(UnityEngine.Purchasing.Product product, UnityEngine.Purchasing.PurchaseFailureReason reason)
    {

    }
}
