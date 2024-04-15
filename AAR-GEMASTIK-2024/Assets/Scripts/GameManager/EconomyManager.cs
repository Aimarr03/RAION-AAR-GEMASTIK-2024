using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    public int currentMoney;

    public event Action<int> UseMoney;
    public event Action<int> gainMoney;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PurchaseSomething(int price)
    {
        if (isPurchasable(price))
        {
            currentMoney -= price;
            UseMoney?.Invoke(currentMoney);
        }
    }
    public bool isPurchasable(int price)
    {
        return currentMoney >= price;
    }
}
