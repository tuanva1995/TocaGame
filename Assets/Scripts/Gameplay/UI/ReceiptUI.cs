using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using TMPro;

public class ReceiptUI : Singleton<ReceiptUI>, IMessageHandle
{
    [SerializeField] Image[] iconItems;
    [SerializeField] TMP_Text[] priceItems;
    [SerializeField] TMP_Text textTotalPrice;
    [SerializeField] GameObject endGamePanel;

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
        gameObject.SetActive(true);
        endGamePanel.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        endGamePanel.SetActive(false);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnChangeState:
                if ((GameState)message.data[0] == GameState.End)
                {
                    Show();
                    int totalPrice = 0;
                    List<ItemInBag> itemInBags = (List<ItemInBag>)message.data[1];
                    for (int i = 0; i < itemInBags.Count; i++)
                    {
                        iconItems[i].sprite = SpriteController.GetUISprite(itemInBags[i].itemId);
                        totalPrice += itemInBags[i].price;
                        priceItems[i].text = itemInBags[i].price.ToString();
                        textTotalPrice.text = totalPrice.ToString();
                    }
                }
                else
                {
                    Hide();
                }
                break;
        }
    }

    public void OnClickHome()
    {
        GameController.Instance.SetupGame();
    }

    public void OnClickRestart()
    {
        Hide();
        GameController.Instance.RestartGame();
    }
}
