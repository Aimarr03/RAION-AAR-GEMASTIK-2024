using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour, IDataPersistance
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
    public void OnGainMoney(int revenue)
    {
        currentMoney += revenue;
        gainMoney?.Invoke(currentMoney);
    }
    public bool isPurchasable(int price)
    {
        return currentMoney >= price;
    }
    public int GetMoneyMultiplierBasedOnTrash(float trashAmount) => (int)(trashAmount * 100);

    public void LoadScene(GameData gameData)
    {
        Debug.Log(gameData.money);
        currentMoney = gameData.money;
    }

    public void SaveScene(ref GameData gameData)
    {
        gameData.money = currentMoney;
        Debug.Log(gameData.money);
    }
}
