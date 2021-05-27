using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public static Action UpdateTextAction;

    public Text ButtonText;
    public Store CurrentStore;
    public ScrollSnapRect CurrentScroll;
    public GameObject NoMoneyPanel;
    public SaveLoad SaveLoad;
    public GameObject StorePanel;

    private void OnEnable()
    {
        UpdateTextAction += UpdateButtonText;
    }

    private void OnDisable()
    {
        UpdateTextAction -= UpdateButtonText;
    }

    public void ButtonAction()
    {
        if (!CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].IsBough)
        {
            Buy();
        }
        else
        {
            Use();
        }
    }

    private void Buy()
    {
        if (MoneyManager.Instance.IsEnoughMoney(CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].ItemPrice))
        {
            MoneyManager.Instance.MinusMoney(CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].ItemPrice);
            CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].IsBough = true;
            UpdateButtonText();
            CurrentStore.UpdateItemSprite();
            SaveLoad.Save();
        }
        else
        {
            NoMoneyPanel.SetActive(true);
        }
    }

    private void Use()
    {
        foreach (var item in CurrentStore.CurrentItemList)
        {
            item.IsSelected = false;
        }
        CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].IsSelected = true;
        PlayerPrefs.SetInt("BallId", CurrentScroll.CurrentPage);
        if (PlayerPrefs.GetInt("FirstStart") == 0)
        {
            PlayerPrefs.SetInt("FirstStart", 1);
        }
        CurrentStore.UpdateItemSprite();
        SaveLoad.Save();
        StorePanel.SetActive(false);
    }

    public void UpdateButtonText()
    {
        if (!CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].IsBough)
        {
            ButtonText.text = "BUY FOR " + CurrentStore.CurrentItemList[CurrentScroll.CurrentPage].ItemPrice;
        }
        else
        {
            ButtonText.text = "USE";
        }
    }
}
