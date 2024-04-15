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
    public void SetGeneralData(PlayerUsableGeneralData generalData, ShopMode mode)
    {
        this.generalData = generalData;
        header.text = generalData.name;
        CanBeUseBuyActionOrNot(generalData.unlocked, mode);
    }
    public void OnOpenDetailedCardView()
    {
        if (detailedCardView.gameObject.activeSelf) return;
        detailedCardView.OpenTab(generalData, ShopManager.instance.shopMode);
    }
    private void CanBeUseBuyActionOrNot(bool unlockable, ShopMode mode)
    {
        ColorBlock colorBlock = thisButton.colors;
        bool canBeInterracted = false;
        switch (mode)
        {
            case ShopMode.Buy:
                canBeInterracted = unlockable;
                break;
            case ShopMode.Upgrade:
                canBeInterracted = !unlockable;
                break;
        }
        colorBlock.normalColor = canBeInterracted ? Color.white : Color.red;
        thisButton.colors = colorBlock;
    }
}
