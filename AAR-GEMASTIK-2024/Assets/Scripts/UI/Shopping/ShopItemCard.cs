using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : MonoBehaviour
{
    public DetailedCardView detailedCardView;
    public PlayerUsableGeneralData generalData;
    public TextMeshProUGUI header;
    public void SetGeneralData(PlayerUsableGeneralData generalData)
    {
        this.generalData = generalData;
        header.text = generalData.name;
    }
    public void OnOpenDetailedCardView()
    {
        if (detailedCardView.gameObject.activeSelf) return;
        detailedCardView.OpenTab(generalData);
    }
}
