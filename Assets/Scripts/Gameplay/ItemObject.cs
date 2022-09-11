using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using BitBenderGames;

public class ItemObject : MonoBehaviour
{
    [SerializeField] string itemId;
    private Vector3 originPos;
    private Vector3 worldPos;

    public void SetupData(Vector3 originPos)
    {
        this.originPos = originPos;
        transform.position = originPos;
    }

    private void OnMouseDown()
    {
        transform.DOKill();
        ItemPlacement.Instance.OnDragItem(itemId);
    }

    private void OnMouseDrag()
    {
        switch (GameController.Instance.State)
        {
            case GameState.ChooseItem:
            case GameState.PutAway:
                worldPos = GameController.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = transform.position.z;
                transform.position = worldPos;
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        RaycastHit2D[] hits = TouchInputHelper.Instance.GetAllRaycastHit();
        switch (GameController.Instance.State)
        {
            case GameState.ChooseItem:
                if (hits.Any(e => e.collider.GetComponent<ItemPlacement>() != null))
                {
                    originPos = transform.position;
                    GameController.Instance.ChangeState(GameState.PriceCheck);
                    GameController.Instance.SetupCurrentItem(itemId);
                }
                else
                {
                    transform.DOMove(originPos, 0.5f).SetEase(Ease.OutCubic);
                }
                ItemPlacement.Instance.OnDropItem();
                break;
            case GameState.PutAway:
                if (hits.Any(e => e.collider.CompareTag(Constants.Tag.BagItem)))
                {
                    GameController.Instance.PutItemToBag(itemId);
                }
                else
                {
                    transform.DOMove(originPos, 0.5f).SetEase(Ease.OutCubic);
                }
                break;
        }
    }
}
