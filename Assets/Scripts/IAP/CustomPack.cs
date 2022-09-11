using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class CustomPack : MonoBehaviour
{
    [SerializeField] string customPackId;
    [SerializeField] TeeIAPButton buyButton;
    public string PackID
    {
        get { return customPackId; }        
    }
    private void OnEnable()
    {
        buyButton.onPurchaseComplete.AddListener(OnCompletePurchase);
    }
    private void OnDisable()
    {
        buyButton.onPurchaseComplete.RemoveListener(OnCompletePurchase);
    }
    public void OnCompletePurchase(Product product)
    {
        DataController.Instance.gameData.iapPackBought.Add(customPackId);
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnBuyIAP, new object[] { customPackId }));
    }
}
