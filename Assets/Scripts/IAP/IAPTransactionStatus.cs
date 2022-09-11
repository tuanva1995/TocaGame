using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Pixelplacement;
#pragma warning disable 0649
public class IAPTransactionStatus : Singleton<IAPTransactionStatus>
{
    [SerializeField] private Text transactionStatus, buyStatus;
    [SerializeField] private GameObject panel;
    private bool autoClose = false;
    private float timeStamp, clickTimeStamp;
    private void Update()
    {
        if (autoClose)
        {
            if (Time.time - timeStamp > 45)
            {
                panel.SetActive(false);
                autoClose = false;
                GetComponent<Animator>().Play("Disappear");
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - clickTimeStamp < 0.25f)
                {
                    panel.SetActive(false);
                    autoClose = false;
                    GetComponent<Animator>().Play("Disappear");
                }
                clickTimeStamp = Time.time;
            }
        }
    }
    public async void OpenTransactionOverlay()
    {
        autoClose = false;
        transactionStatus.text = "Processing";
        buyStatus.text = "";
        panel.SetActive(true);
        GetComponent<Animator>().Play("Appear");
        await System.Threading.Tasks.Task.Delay(15000);
        autoClose = true;
        timeStamp = Time.time;
    }
    public void ShowStatus(bool isSuccess, UnityEngine.Purchasing.Product product, PurchaseFailureReason reason = PurchaseFailureReason.Unknown)
    {
        if (isSuccess)
        {
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
            panel.SetActive(false);
            autoClose = false;
        }
        else
        {
            transactionStatus.text = "Failed";
            string message = "";
            if (product != null)
                message = reason.ToString() + "\n\n";
            buyStatus.text = message + "<color=#005ed2>Double tap to close.</color>";
            autoClose = true;
        }
        timeStamp = Time.time;
    }
}
