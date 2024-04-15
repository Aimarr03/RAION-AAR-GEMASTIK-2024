using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : MonoBehaviour
{
    public Button thisButton;
    public DetailedCardView detailedCardView;
    public PlayerUsableGeneralData generalData;
    public TextMeshProUGUI header;
    public TextMeshProUGUI priceText;
    private bool isBuyable;
    public void SetGeneralData(PlayerUsableGeneralData generalData, ShopMode mode)
    {
        this.generalData = generalData;
        header.text = generalData.name;
        IsBuyableOrNot(mode);
        CanBeUseBuyActionOrNot(generalData.unlocked, mode);
    }
    public void SetGeneralData(ShopMode mode)
    {
        IsBuyableOrNot(mode);
        CanBeUseBuyActionOrNot(generalData.unlocked, mode);
    }
    public void OnOpenDetailedCardView()
    {
        if (detailedCardView.gameObject.activeSelf) return;
        detailedCardView.OpenTab(generalData, ShopManager.instance.shopMode, isBuyable);
    }
    private void CanBeUseBuyActionOrNot(bool unlockable, ShopMode mode)
    {
        ColorBlock colorBlock = thisButton.colors;
        bool canBeInterracted = false;
        switch (mode)
        {
            case ShopMode.Buy:
                canBeInterracted = !unlockable;
                break;
            case ShopMode.Upgrade:
                canBeInterracted = unlockable;
                break;
        }
        colorBlock.normalColor = canBeInterracted ? Color.white : Color.red;
        if (!canBeInterracted) priceText.text = "";
        thisButton.colors = colorBlock;
    }
    private void IsBuyableOrNot(ShopMode mode)
    {
        int price = 0;
        switch (mode)
        {
            case ShopMode.Buy: price = generalData.buyPrice; break;
            case ShopMode.Upgrade: price = generalData.upgradePrice; break;
        }
        priceText.text = price.ToString();
        isBuyable = EconomyManager.Instance.isPurchasable(price);
        priceText.color = isBuyable ? Color.black : Color.red;
    }
    public int UpgradeModeComparison(ShopItemCard other)
    {
        int boolComparison = generalData.unlocked.CompareTo(other.generalData.unlocked);
        if (boolComparison != 0)
        {
            return -boolComparison;
        }
        else
        {
            return generalData.name.CompareTo(other.generalData.name);
        }
    }
    public int BuyModeComparison(ShopItemCard other)
    {
        int boolComparison = generalData.unlocked.CompareTo(other.generalData.unlocked);
        if (boolComparison != 0)
        {
            return boolComparison;
        }
        else
        {
            return generalData.name.CompareTo(other.generalData.name);
        }
    }
}
