using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriceCheckUI : MonoBehaviour, IMessageHandle
{
    [SerializeField] TMP_Text textCurrentPrice;
    [SerializeField] Button[] buttonPrices;
    [SerializeField] GameObject coinStorage;

    private void Awake()
    {
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnChangeState, this);
    }

    private void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnChangeState, this);
    }

    public void Show()
    {
        textCurrentPrice.text = "";
        foreach (Button button in buttonPrices)
        {
            button.interactable = true;
        }
        coinStorage.SetActive(true);
    }

    public void Hide()
    {
        foreach (Button button in buttonPrices)
        {
            button.interactable = false;
        }
        coinStorage.SetActive(false);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnChangeState:
                if ((GameState)message.data[0] == GameState.PriceCheck)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
                break;
        }
    }

    public void OnClickButtonPrice(int price)
    {
        textCurrentPrice.text = price.ToString();
        GameController.Instance.SetupCurrentItemPrice(price);
        GameController.Instance.ChangeState(GameState.PriceCheck);
    }
}
