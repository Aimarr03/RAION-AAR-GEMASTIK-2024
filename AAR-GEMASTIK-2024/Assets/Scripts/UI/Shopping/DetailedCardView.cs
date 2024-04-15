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
    private PlayerUsableGeneralData generalData;
    private bool isBuyable;
    public void CloseTab() => gameObject.SetActive(false);

    public void OpenTab(PlayerUsableGeneralData playerUsableGeneralData, ShopMode mode, bool isBuyable)
    {
        gameObject.SetActive(true);
        this.generalData = playerUsableGeneralData;
        headerText.text = playerUsableGeneralData.name;
        contentText.text = playerUsableGeneralData.description;
        levelText.text = playerUsableGeneralData.level.ToString();
        CanBeUseBuyActionOrNot(playerUsableGeneralData.unlocked, mode);
        SetButtonIsBuyableOrNot(isBuyable);
    }
    private void CanBeUseBuyActionOrNot(bool unlockable, ShopMode mode)
    {
        TextMeshProUGUI buttonText = BuyAction.GetComponentInChildren<TextMeshProUGUI>();
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
                    buttonText.text = "Upgrade " + generalData.upgradePrice;
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
        if (!isBuyable)
        {
            BuyAction.interactable = false;
            TextMeshProUGUI textButton = BuyAction.GetComponentInChildren<TextMeshProUGUI>();
            textButton.text = "Not Enough Gold";
        }
    }
}
