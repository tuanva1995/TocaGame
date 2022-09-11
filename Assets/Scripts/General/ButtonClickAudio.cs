using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class ButtonClickAudio : MonoBehaviour
{
    [SerializeField] private AudioClip btnClickAudio;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(OnClick);
    }
    public void OnClick()
    {
        AudioController.Instance.PlaySfx(btnClickAudio);
    }
}
