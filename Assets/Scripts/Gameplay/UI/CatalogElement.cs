using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CatalogElement : MonoBehaviour
{
    [ReadOnly]
    public string ItemId { get; private set; }
    private UnityAction<string> onClick;

    public void SetupData(ItemData data, UnityAction<string> onClick)
    {
        this.ItemId = data.id;
        this.onClick = onClick;
    }

    public void OnClickElement()
    {
        onClick?.Invoke(ItemId);
    }
}
