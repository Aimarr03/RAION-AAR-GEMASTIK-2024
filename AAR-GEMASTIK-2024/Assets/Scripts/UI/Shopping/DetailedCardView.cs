using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailedCardView : MonoBehaviour
{
    public Button BuyAction;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI levelText;
    public ItemBaseSO itemSO;
    public static event Action OnBoughtSomething;
    public static event Action OnUpgradedSomething;
    private PlayerUsableGeneralData generalData
    {
        get => itemSO.generalData;
    }
    private bool isBuyable;
    public void CloseTab() => gameObject.SetActive(false);

    public void OpenTab(ItemBaseSO itemSO, ShopMode mode, bool isBuyable)
    {
        this.itemSO = itemSO;
        gameObject.SetActive(true);
        headerText.text = generalData.name;
        contentText.text = generalData.description;
        if (itemSO is ConsumableItemSO consumableItemSO) levelText.text = consumableItemSO.quantity.ToString();
        else levelText.text = "LVL" + generalData.level.ToString();
        SetButtonIsBuyableOrNot(isBuyable);
        CanUseBuyActionOrNot(generalData.unlocked, mode);
        SetAction(isBuyable, mode);
    }
    private void CanUseBuyActionOrNot(bool unlockable, ShopMode mode)
    {
        if (!BuyAction.interactable) return;
        if (itemSO is ConsumableItemSO) unlockable = !unlockable;
        TextMeshProUGUI buttonText = BuyAction.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        switch(mode)
        {
            case ShopMode.Buy:
                BuyAction.interactable = !unlockable;
                if (BuyAction.interactable)
                {
                    buttonText.text = "Buy " + generalData.buyPrice;
                }
                else
                {
                    buttonText.text = "Already Bought!";
                }
                break;
            case ShopMode.Upgrade: 
                BuyAction.interactable = unlockable;
                if (BuyAction.interactable)
                {
                    buttonText.text = "Upgrade " + generalData.totalUpgradePrice;
                }
                else
                {
                    buttonText.text = "Cannot be Upgraded";
                }
                break;
        }
    }
    private void SetButtonIsBuyableOrNot(bool input)
    {
        isBuyable = input;
        switch (ShopManager.instance.shopMode)
        {
            case ShopMode.Buy:
                isBuyable = EconomyManager.Instance.isPurchasable(itemSO.generalData.buyPrice);
                break;
            case ShopMode.Upgrade:
                isBuyable = EconomyManager.Instance.isPurchasable(itemSO.generalData.totalUpgradePrice);
                break;
        }
        BuyAction.interactable = isBuyable;
        if (!isBuyable)
        {
            BuyAction.interactable = false;
            TextMeshProUGUI textButton = BuyAction.GetComponentInChildren<TextMeshProUGUI>();
            textButton.text = "Not Enough Gold";
        }
    }
    private void SetAction(bool isBuyable, ShopMode mode)
    {
        if (!isBuyable) return;
        BuyAction.onClick.RemoveAllListeners();
        BuyAction.onClick.AddListener(UseMoney);
        switch (mode)
        {
            case ShopMode.Buy:
                BuyAction.onClick.AddListener(OnBuy);
                break;
            case ShopMode.Upgrade:
                BuyAction.onClick.AddListener(OnUpgrade);
                break;
        }
    }
    private void UseMoney()
    {
        int price = 0;
        switch (ShopManager.instance.shopMode)
        {
            case ShopMode.Buy: price = generalData.buyPrice; break;
            case ShopMode.Upgrade: price = generalData.totalUpgradePrice; break;
        }
        EconomyManager.Instance.PurchaseSomething(price);
    }
    private void OnBuy()
    {
        if(itemSO is ConsumableItemSO)
        {
            OnBuyToGet();
        }
        else
        {
            OnBuyToUnlock();
        }
        
    }
    private void OnBuyToUnlock()
    {
        itemSO.generalData.unlocked = true;
        BuyAction.interactable = false;
        TextMeshProUGUI buttonText = BuyAction.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Already Bought!";
        gameObject.SetActive(false);
        OnBoughtSomething?.Invoke();
    }
    private void OnBuyToGet()
    {
        ConsumableItemSO consumableItemSO = itemSO as ConsumableItemSO;
        consumableItemSO.quantity++;
        OnUpdateBuy(consumableItemSO.quantity);
        OnBoughtSomething?.Invoke();
    }
    private void OnUpgrade()
    {
        itemSO.generalData.level++;
        OnUpdateUpgrade();
        OnUpgradedSomething?.Invoke();
    }
    private void OnUpdateUpgrade()
    {
        levelText.text = "LVL " + generalData.level.ToString();
        int newPrice = generalData.totalUpgradePrice;
        bool isBuyable = EconomyManager.Instance.isPurchasable(newPrice);
        TextMeshProUGUI buttonText = BuyAction.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Upgrade " + newPrice;
        SetButtonIsBuyableOrNot(isBuyable);
    }
    private void OnUpdateBuy(int quantity)
    {
        bool isBuyable = EconomyManager.Instance.isPurchasable(generalData.buyPrice);
        levelText.text = quantity.ToString();
        SetButtonIsBuyableOrNot(isBuyable);
    }
}
