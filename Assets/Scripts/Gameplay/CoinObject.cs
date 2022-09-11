using BitBenderGames;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinObject : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 worldPos;
    private int price;

    public void SetupData(Vector3 originPos)
    {
        this.originPos = originPos;
        transform.position = originPos;
    }

    private void OnMouseDown()
    {
        transform.DOKill();
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
            case GameState.Payment:
                if (hits.Any(e => e.collider.CompareTag(Constants.Tag.BagCoin)))
                {
                    GameController.Instance.PutCoinToBag();
                }
                else
                {
                    transform.DOMove(originPos, 0.5f).SetEase(Ease.OutCubic);
                }
                break;
        }
    }
}
