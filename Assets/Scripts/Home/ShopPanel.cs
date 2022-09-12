using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using DG.Tweening;
using TMPro;
using System.Linq;
using System;
using UnityEngine.Purchasing;
using Pixelplacement.TweenSystem;
#pragma warning disable 0649

public class ShopPanel : Singleton<ShopPanel>, IMessageHandle
{
    [SerializeField] Transform gemIcon;
    [SerializeField] Transform popup;
    [SerializeField] AudioClip openClip, closeClip;
    [SerializeField] TMP_Text textGem;
    private Action onClose;
    private TweenBase addGemTween;

    private void Awake()
    {
        MessageManager.Instance.AddSubscriber(TeeMessageType.OnDataChange, this);
        popup.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        MessageManager.Instance.RemoveSubscriber(TeeMessageType.OnDataChange, this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        popup.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        AudioController.Instance.PlaySfx(openClip);
        textGem.text = DataController.Instance.Gem.ToString();
    }

    public void OnClickClose()
    {
        AudioController.Instance.PlaySfx(closeClip);
        popup.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            onClose?.Invoke();
            gameObject.SetActive(false);
        });
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnDataChange:
                if (DataController.Instance.Gem > int.Parse(textGem.text))
                {
                    addGemTween?.Stop();
                    int origin = int.Parse(textGem.text);
                    int amountToAdd = DataController.Instance.Gem - origin;
                    addGemTween = Pixelplacement.Tween.Value(0, amountToAdd, (int value) =>
                    {
                        textGem.text = (origin + value).ToString();
                    }, Mathf.Clamp(0.1f * amountToAdd, 0, 3), 1, Pixelplacement.Tween.EaseLinear);
                }
                else
                {
                    textGem.text = DataController.Instance.Gem.ToString();
                }
                break;
        }
    }
    public void OnCompletePurchase(Product product)//handle effect
    {
        Pack pack = IAPPackController.Instance.GetPackById(product.definition.id);
        TeeIAPButton[] buttons = GetComponentsInChildren<TeeIAPButton>();
        TeeIAPButton activeBtn = buttons.First(e => e.productId == product.definition.id);
        DataController.Instance.Gem += pack.gem;
        DataController.Instance.Coin += pack.coin;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
    }
}
