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

    public void OpenTab(PlayerUsableGeneralData playerUsableGeneralData)
    {
        gameObject.SetActive(true);
        headerText.text = playerUsableGeneralData.name;
        contentText.text = playerUsableGeneralData.description;
        levelText.text = playerUsableGeneralData.level.ToString();
    }
}
