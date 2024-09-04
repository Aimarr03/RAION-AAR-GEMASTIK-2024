using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    private void Start()
    {
        DetailedCardView.OnUpgradedSomething += UpdateData;
        DetailedCardView.OnBoughtSomething += UpdateData;
        this.thisButton.onClick.AddListener(SoundEffectClick);
        thisButton.onClick.AddListener(UI_ConversationManager.Instance.PlayDetailedCard);
        RectTransform rect = GetComponent<RectTransform>();
        float localY = rect.anchoredPosition.y;
    }
    private void OnDestroy()
    {
        DetailedCardView.OnUpgradedSomething -= UpdateData;
        DetailedCardView.OnBoughtSomething -= UpdateData;
        GetComponent<RectTransform>().DOKill();
    }

    private void UpdateData()
    {
        Debug.Log(itemSO);
        Debug.Log(generalData.unlocked);
        Debug.Log(ShopManager.instance.shopMode);
        CanBeUseBuyActionOrNot(generalData.unlocked, ShopManager.instance.shopMode);
    }

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
        //Debug.Log(itemSO);
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
        UnavailableContainer.gameObject.SetActive(!canBeInterracted);
        UnavailableContainer.GetComponentInChildren<TextMeshProUGUI>().text = text;
        icon.sprite = generalData.icon;

        GetComponent<Image>().color = !canBeInterracted ? Color.white : unavailableGrey;
        icon.color = canBeInterracted ? Color.white : unavailableGrey;
        IsBuyableOrNotVisually(mode, canBeInterracted);
    }
    private void SoundEffectClick()
    {
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.interractable, 2.5f);
        thisButton.onClick.RemoveListener(UI_ConversationManager.Instance.PlayDetailedCard);
    }
    private void IsBuyableOrNotVisually(ShopMode mode, bool canBeInterracted)
    {
        int price = 0;
        switch (mode)
        {
            case ShopMode.Buy: price = generalData.buyPrice; break;
            case ShopMode.Upgrade: price = generalData.totalUpgradePrice; break;
        }
        isBuyable = EconomyManager.Instance.isPurchasable(price);
        if (!canBeInterracted)
        {
            isBuyable = false;
        }
        priceText.text = price.ToString();
        priceText.color = isBuyable ? Color.white : unavailableRed;

        header.text = itemSO.generalData.name;
        if(itemSO is ConsumableItemSO consumableItemSO)
        {
            header.text += " " + consumableItemSO.itemTier;
        }
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
