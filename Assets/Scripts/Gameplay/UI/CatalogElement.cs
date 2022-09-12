using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class CatalogElement : MonoBehaviour
{
    [SerializeField] Image icon;
    private UnityAction<string> onClick;
    [ReadOnly]
    public string ItemId { get; private set; }

    public void SetupData(ItemData data, UnityAction<string> onClick)
    {
        this.ItemId = data.id;
        this.onClick = onClick;
        icon.sprite = SpriteController.GetUISprite(data.id);
    }

    public void OnClickElement()
    {
        onClick?.Invoke(ItemId);
    }
}
