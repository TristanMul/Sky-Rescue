using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public Text MoneyText;

    [HideInInspector]
    public int MoneyValue;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MoneyValue = PlayerPrefs.GetInt("Star");
        UpdateMoneyUI();
    }

    public void AddMoney(int value)
    {
        MoneyValue += value;
        UpdateMoneyUI();
    }

    public void MinusMoney(int value)
    {
        MoneyValue -= value;
        UpdateMoneyUI();
    }

    public bool IsEnoughMoney (int PriceValue)
    {
        if (MoneyValue >= PriceValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateMoneyUI()
    {
        MoneyText.text = MoneyValue.ToString();
        PlayerPrefs.SetInt("Star", MoneyValue);
    }
}
