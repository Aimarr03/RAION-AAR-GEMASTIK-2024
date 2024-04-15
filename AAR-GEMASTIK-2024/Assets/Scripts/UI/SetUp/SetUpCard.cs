using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class SetUpCard : MonoBehaviour
{
    public ItemBaseSO itemBaseSO;
    public Button thisButton;
    public PlayerUsableGeneralData generalData
    {
        get => itemBaseSO.generalData;
    }
    public ItemType type;
    public TextMeshProUGUI textHeader;
    public void SetUpData(ItemBaseSO so, ItemType type)
    {
        this.itemBaseSO = so;
        this.type = type;
        textHeader.text = generalData.name;
        thisButton.interactable = generalData.unlocked;
        UpdateColor();
    }
    private void Start()
    {
        DetailedCardView.OnBoughtSomething += UpdateData;
    }
    private void UpdateData()
    {
        bool isUnlocked = generalData.unlocked;
        thisButton.interactable = isUnlocked;
        UpdateColor();   
    }
    private void UpdateColor()
    {
        ColorBlock colorBlock = this.thisButton.colors;
        colorBlock.normalColor = generalData.unlocked ? Color.white : Color.gray;
        thisButton.colors = colorBlock;
    }
    public void SetPlayerData()
    {
        switch(type)
        {
            case ItemType.Item:
                ConsumableItemSO itemSO = itemBaseSO as ConsumableItemSO;
                GameManager.Instance.chosenConsumableItemSO = itemSO;
                break;
            case ItemType.Weapon:
                WeaponSO weaponSO = itemBaseSO as WeaponSO;
                GameManager.Instance.chosenWeaponSO = weaponSO;
                break;
            case ItemType.Ability: 
                AbilitySO abilitySO = itemBaseSO as AbilitySO;
                GameManager.Instance.chosenAbilitySO = abilitySO;
                break;
        }
    }
}
