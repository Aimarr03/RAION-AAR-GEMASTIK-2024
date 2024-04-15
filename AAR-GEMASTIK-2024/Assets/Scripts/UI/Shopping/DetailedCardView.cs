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
    public void CloseTab() => gameObject.SetActive(false);

    public void OpenTab(PlayerUsableGeneralData playerUsableGeneralData, ShopMode mode)
    {
        gameObject.SetActive(true);
        headerText.text = playerUsableGeneralData.name;
        contentText.text = playerUsableGeneralData.description;
        levelText.text = playerUsableGeneralData.level.ToString();
        CanBeUseBuyActionOrNot(playerUsableGeneralData.unlocked, mode);
    }
    private void CanBeUseBuyActionOrNot(bool unlockable, ShopMode mode)
    {
        switch(mode)
        {
            case ShopMode.Buy:
                BuyAction.interactable = unlockable;
                break;
            case ShopMode.Upgrade: 
                BuyAction.interactable = !unlockable;
                break;
        }
    }
}
