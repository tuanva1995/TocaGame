using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pixelplacement;

public class GameController : Singleton<GameController>
{
    [SerializeField] List<DishObject> dishes = new List<DishObject>();
    private List<ItemInBag> itemInBag = new List<ItemInBag>();
    private int coinInBag = 0;
    public GameState State { get; private set; }
    private Camera _camera;
    public Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }

    private void Awake()
    {
        SetupGame();
    }

    public void SetupGame()
    {
        ChangeState(GameState.Catalog);
        // Renew State
        foreach (DishObject dishObject in dishes)
        {
            dishObject.SetupData("");
        }
        itemInBag = new List<ItemInBag>();
    }

    public void SetupDish(int dishId, string itemId)
    {
        DishObject dish = dishes.Find(e => e.dishId == dishId);
        if (dish != null)
        {
            dish.SetupData(itemId);
        }
        if (dishes.FindAll(e => !e.HasInit).Count == 0)
        {
            StartGame();
        }
    }

    public void SetupCurrentItem(string itemId)
    {
        itemInBag.Add(new ItemInBag(itemId));
    }

    public void SetupCurrentItemPrice(int price)
    {
        itemInBag[itemInBag.Count - 1].price = price;
    }

    public void PutItemToBag(string itemId)
    {
        if (itemInBag.Count >= 3)
        {
            ChangeState(GameState.End);
        }
        else
        {
            ChangeState(GameState.ChooseItem);
        }
    }

    public void PutCoinToBag()
    {
        coinInBag++;
        if (coinInBag >= itemInBag[itemInBag.Count - 1].price)
        {
            ChangeState(GameState.PutAway);
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.ChooseItem);
    }

    public void ChangeState(GameState gameState)
    {
        State = gameState;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnChangeState, new object[] { gameState, itemInBag }));
    }
}

[Serializable]
public class ItemInBag
{
    public string itemId;
    public int price;

    public ItemInBag(string itemId)
    {
        this.itemId = itemId;
    }
}

[Serializable]
public enum GameState
{
    Catalog,
    ChooseItem,
    PriceCheck,
    Payment,
    PutAway,
    End
}
