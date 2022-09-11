using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class CatalogUI : Singleton<CatalogUI>
{
    [SerializeField] GameObject buttonNext, buttonPrevious;
    [SerializeField] CatalogElement[] elements;
    [SerializeField] GameObject[] pageDots;
    private ItemData[] data;
    private int currentDishId;
    private int _page;
    private int Page 
    {
        get { return _page; }
        set { _page = Mathf.Clamp(_page, 1, 999); }
    }

    private void Awake()
    {
        data = JsonHelper.ArrayFromJson<ItemData>(Resources.Load<TextAsset>("CatalogData").text);
    }

    public void Show(int dishId)
    {
        currentDishId = dishId;
        Page = 1;
        buttonNext.SetActive(data.Length > 12);
        buttonPrevious.SetActive(false);
        ChangePage();
    }

    public void OnClickItem(string itemId)
    {
        GameController.Instance.SetupDish(currentDishId, itemId);
        OnClickClose();
    }
    
    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OnClickNext()
    {
        Page++;
        buttonNext.SetActive(Page * 12 >= data.Length);
        ChangePage();
    }

    public void OnClickPrevious()
    {
        Page--;
        buttonPrevious.SetActive(Page == 1);
        ChangePage();
    }

    // Mỗi trang gồm tối đa 12 item
    public void ChangePage()
    {
        int itemThisPage = data.Length - (Page - 1) * 12;
        for (int i = 0; i < Mathf.Min(12, itemThisPage); i++)
        {
            // E.g: Page = 2, display item from (2 - 1) * 12 -> 24 (max)
            int index = (Page - 1) * 12 + i;
            elements[i].gameObject.SetActive(true);
            elements[i].SetupData(data[index], OnClickItem);
        }
        for (int i = itemThisPage; i < elements.Length; i++)
        {
            elements[i].gameObject.SetActive(false);
        }

        foreach (GameObject pageDot in pageDots)
        {
            pageDot.SetActive(false);
        }
        pageDots[_page - 1].SetActive(true);
    }
}
