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
    public ItemBaseSO itemSO;
    public PlayerUsableGeneralData generalData
    {
        get => itemSO.generalData;
    }
    [Header("Text")]
    public TextMeshProUGUI header;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI levelText;
    [Header("Additional Component")]
    public Transform UnavailableContainer;
    public Image icon;
    [Header("Color")]
    public Color unavailableRed;
    public Color unavailableGrey;
    private bool isBuyable;
    public void SetGeneralData(ItemBaseSO itemBaseSO, ShopMode mode)
    {
        itemSO = itemBaseSO;
        CanBeUseBuyActionOrNot(generalData.unlocked, mode);
    }
    public void SetGeneralData(ShopMode mode)
    {
        CanBeUseBuyActionOrNot(generalData.unlocked, mode);
    }
    public void OnOpenDetailedCardView()
    {
        if (detailedCardView.gameObject.activeSelf) return;
        detailedCardView.OpenTab(itemSO, ShopManager.instance.shopMode, isBuyable);
    }
    private void CanBeUseBuyActionOrNot(bool unlockable, ShopMode mode)
    {
        bool canBeInterracted = false;
        string text = "";
        switch (mode)
        {
            case ShopMode.Buy:
                canBeInterracted = !unlockable;
                text = "Already Bought";
                break;
            case ShopMode.Upgrade:
                canBeInterracted = unlockable;
                text = "Not Upgradable";
                break;
        }
        if (itemSO is ConsumableItemSO) canBeInterracted = !canBeInterracted;
        UnavailableContainer.gameObject.SetActive(canBeInterracted);
        UnavailableContainer.GetComponentInChildren<TextMeshProUGUI>().text = text;


        GetComponent<Image>().color = canBeInterracted ? Color.white : unavailableGrey;
        icon.color = canBeInterracted ? Color.white : unavailableGrey;

        IsBuyableOrNotVisually(mode);
    }
    private void IsBuyableOrNotVisually(ShopMode mode)
    {
        int price = 0;
        switch (mode)
        {
            case ShopMode.Buy: price = generalData.buyPrice; break;
            case ShopMode.Upgrade: price = generalData.upgradePrice; break;
        }
        isBuyable = EconomyManager.Instance.isPurchasable(price);
        
        priceText.text = price.ToString();
        priceText.color = isBuyable ? Color.white : unavailableRed;

        header.text = itemSO.generalData.name;
        header.color = isBuyable ? Color.white : unavailableRed;

        string level = itemSO.generalData.level.ToString();
        levelText.text = "lvl "+level;
        levelText.color = isBuyable ? Color.white : unavailableGrey;

        header.color = isBuyable ? Color.white : unavailableRed;
        transform.GetComponent<Image>().color = isBuyable ? Color.white : unavailableGrey;

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
