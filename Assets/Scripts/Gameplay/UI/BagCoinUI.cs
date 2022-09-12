using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class BagCoinUI : Singleton<BagCoinUI>, IMessageHandle
{
    private void Awake()
    {
        MessageManager.Instance.AddSubscriber(TeeMessageType.OnChangeState, this);
    }

    private void OnDestroy()
    {
        MessageManager.Instance.RemoveSubscriber(TeeMessageType.OnChangeState, this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnChangeState:
                if ((GameState)message.data[0] != GameState.ChooseItem)
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
}
