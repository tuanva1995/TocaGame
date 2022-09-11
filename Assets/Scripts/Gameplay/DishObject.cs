using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishObject : MonoBehaviour
{
    public int dishId;
    [Tooltip("Objects will be spawned inside this bound")]
    [SerializeField] SpriteRenderer bound;
    private string itemId;
    public bool HasInit { get { return !string.IsNullOrEmpty(itemId); } }
    public string ItemId => itemId;

    private void OnEnable()
    {
        itemId = "";
    }

    public void SetupData(string itemId)
    {
        this.itemId = itemId;
        int amount = Random.Range(5, 11);
        for (int i = 0; i < amount; i++)
        {
            ItemObject item = ObjectPool.Spawn<ItemObject>(itemId);
            Vector3 pos = GetRandomPosInBound();
            pos.z = - 0.01f * i;
            item.SetupData(pos);
        }
    }

    public void OnClick()
    {
        if (string.IsNullOrEmpty(itemId) && GameController.Instance.State == GameState.Catalog)
            CatalogUI.Instance.Show(dishId);
    }

    private Vector2 GetRandomPosInBound()
    {
        float randomX = Random.Range(bound.bounds.min.x, bound.bounds.max.x);
        float randomY = Random.Range(bound.bounds.min.y, bound.bounds.max.y);
        return new Vector2(randomX, randomY);
    }
}
