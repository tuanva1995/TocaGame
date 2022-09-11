using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BitBenderGames;
using UnityEngine.UI;
using Pixelplacement;

public class ItemPlacement : Singleton<ItemPlacement>
{
    [SerializeField] Image imageItem;

    private void OnEnable()
    {
        imageItem.gameObject.SetActive(false);
    }

    public void OnDragItem(string id)
    {
        imageItem.gameObject.SetActive(true);
        imageItem.sprite = SpriteController.GetUISprite(id + "_shadow");
        imageItem.SetNativeSize();
    }

    public void OnDropItem()
    {
        imageItem.gameObject.SetActive(false);
    }
}
