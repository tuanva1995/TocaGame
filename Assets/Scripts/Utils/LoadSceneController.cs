using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using TMPro;
#pragma warning disable 0649
public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private RectTransform mask;
    [SerializeField] private TextMeshProUGUI loadingText;
    private float originMaskWidth, originMaskHeight;
    private void Start()
    {
        originMaskWidth = mask.sizeDelta.x;
        originMaskHeight = mask.sizeDelta.y;
        mask.sizeDelta = new Vector2(0, originMaskHeight);
        loadingText.text = "0%";
        Fade(0, 1, 0.5f);
    }
    public void FadeOut()
    {
        Fade(1, 0, 0.5f);
    }
    private void Fade(float startAlpha, float endAlpha, float duration)
    {
        var startColor = new Color(1, 1, 1, startAlpha);
        var endColor = new Color(1, 1, 1, endAlpha);
        Tween.Value(startColor, endColor, UpdateImageColor, duration, 0, Tween.EaseIn, Tween.LoopType.None, null, null, false);
    }
    private void UpdateImageColor(Color color)
    {
        foreach (Image image in images)
            image.color = color;
        loadingText.color = color;
    }
    public void UpdateLoadingBar(float percent)
    {
        loadingText.text = (int)(percent * 100) + "%";
        mask.sizeDelta = new Vector2(percent * originMaskWidth, originMaskHeight);
    }
}
