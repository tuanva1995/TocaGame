﻿using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine.Purchasing
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Unity IAP/Tee IAP Button")]
    [HelpURL("https://docs.unity3d.com/Manual/UnityIAP.html")]
    public class TeeIAPButton : IAPButton
    {
        public const string PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAh5kIUwsLcvas2CyxXMMbKITTeWxA5H3QGw1O/JR2Lks+uWBR6g38tJ51jLejYpoaKEqG4lbyGiwfJ+/i18UW5Ai7w4d2LnITLAb3TW8YtFFvJaFAqW/xG9JlwfAcc3P4g5ROCorSXzuep3yMyx5kDAoD/Ny6QW5nv5J8Sc5PMluJgJaPD6p2WT3BOVLYaLgvBmvBrarDzucDPgCSoxAw/pTtCjthi9aIF/m9XXVeiiMl8eHDlVxwCrxbjdGZHep3z5Jw7HxD6xd/94I8ELxKgXfxaDj14ZFWx7lEYOSwoX4IvRVdzNZwqxgzUH1NPa+V8AnRyT41EfwN+HCLv+vPJwIDAQAB";

        [Tooltip("[Optional] Displays the localized title from the app store")]
        public TextMeshProUGUI titleTextTMP;

        [Tooltip("[Optional] Displays the localized description from the app store")]
        public TextMeshProUGUI descriptionTextTMP;

        [Tooltip("[Optional] Displays the localized price from the app store")]
        public TextMeshProUGUI priceTextTMP;
        void Start()
        {
            Button button = GetComponent<Button>();

            if (buttonType == ButtonType.Purchase)
            {
                if (button)
                {
                    button.onClick.AddListener(PurchaseProduct);
                    onPurchaseComplete.AddListener(OnCompletePurchase);
                    onPurchaseFailed.AddListener(OnFailPurchase);
                }

                if (string.IsNullOrEmpty(productId))
                {
                    Debug.LogError("IAPButton productId is empty");
                }

                if (!CodelessIAPStoreListener.Instance.HasProductInCatalog(productId))
                {
                    Debug.LogWarning("The product catalog has no product with the ID \"" + productId + "\"");
                }
            }
            else if (buttonType == ButtonType.Restore)
            {
                if (button)
                {
                    button.onClick.AddListener(Restore);
                }
            }
        }

        void OnEnable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.AddButton(this);
                if (CodelessIAPStoreListener.initializationComplete)
                {
                    UpdateText();
                }
            }
        }

        void OnDisable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.RemoveButton(this);
            }
        }
        public void OnCompletePurchase(Product product)
        {
            IAPTransactionStatus.Instance.ShowStatus(true, product);
            PlayerPrefs.SetInt(product.definition.id + "_bought_time_stamp", System.DateTime.Today.DayOfYear);
            DataController.Instance.gameData.iapPackBought.Add(product.definition.id);
            DataController.Instance.SaveData();
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnBuyIAP, new object[] { product.definition.id }));
        }
        public void OnFailPurchase(Product product, PurchaseFailureReason reason)
        {
            IAPTransactionStatus.Instance.ShowStatus(false, product, reason);
        }
        void PurchaseProduct()
        {
            if (buttonType == ButtonType.Purchase)
            {
                Debug.Log("IAPButton.PurchaseProduct() with product ID: " + productId);
                IAPTransactionStatus.Instance.OpenTransactionOverlay();
                CodelessIAPStoreListener.Instance.InitiatePurchase(productId);
            }
        }

        void Restore()
        {
            if (buttonType == ButtonType.Restore)
            {
                if (Application.platform == RuntimePlatform.WSAPlayerX86 ||
                    Application.platform == RuntimePlatform.WSAPlayerX64 ||
                    Application.platform == RuntimePlatform.WSAPlayerARM)
                {
                    CodelessIAPStoreListener.Instance.GetStoreExtensions<IMicrosoftExtensions>()
                        .RestoreTransactions();
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                         Application.platform == RuntimePlatform.OSXPlayer ||
                         Application.platform == RuntimePlatform.tvOS)
                {
                    CodelessIAPStoreListener.Instance.GetStoreExtensions<IAppleExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                    StandardPurchasingModule.Instance().appStore == AppStore.GooglePlay)
                {
                    CodelessIAPStoreListener.Instance.GetStoreExtensions<IGooglePlayStoreExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else
                {
                    Debug.LogWarning(Application.platform.ToString() +
                                     " is not a supported platform for the Codeless IAP restore button");
                }
            }
        }

        void OnTransactionsRestored(bool success)
        {
            Debug.Log("Transactions restored: " + success);
        }

        internal void UpdateText()
        {
            var product = CodelessIAPStoreListener.Instance.GetProduct(productId);
            if (product != null)
            {
                if (titleTextTMP != null)
                {
                    titleTextTMP.text = product.metadata.localizedTitle;
                }

                if (descriptionTextTMP != null)
                {
                    descriptionTextTMP.text = product.metadata.localizedDescription;
                }
                if (priceTextTMP != null)
                {
                    priceTextTMP.text = product.metadata.localizedPriceString.Trim();
                }
            }
        }
    }
}
