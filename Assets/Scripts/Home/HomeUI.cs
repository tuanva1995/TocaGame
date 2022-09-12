using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    private bool hasClick = false;

    private void Awake()
    {
        hasClick = false;
    }

    public void OnClickPlay()
    {
        if (hasClick)
            return;
        hasClick = true;
        SceneController.Instance.LoadScene("Gameplay");
    }

    public void OnClickSetting()
    {
        SettingPanel.Instance.Show();
    }

    public void OnClickShop()
    {
        ShopPanel.Instance.Show();
    }
}
