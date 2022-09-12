using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SettingPanel : Pixelplacement.Singleton<SettingPanel>
{
    [SerializeField] Transform popup;
    [SerializeField] GameObject slashMusic;
    [SerializeField] GameObject slashSound;

    public void Show()
    {
        popup.transform.localScale = Vector2.zero;
        gameObject.SetActive(true);
        popup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        popup.transform.localScale = Vector3.zero;
        slashMusic.SetActive(!AudioController.Instance.Music);
        slashSound.SetActive(!AudioController.Instance.SFX);
    }

    public void OnClickMusic()
    {
        slashMusic.SetActive(!slashMusic.activeSelf);
        AudioController.Instance.Music = !AudioController.Instance.Music;
    }

    public void OnClickSound()
    {
        slashSound.SetActive(!slashSound.activeSelf);
        AudioController.Instance.SFX = !AudioController.Instance.SFX;
    }

    public void OnClickClose()
    {
        popup.DOKill();
        popup.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}
