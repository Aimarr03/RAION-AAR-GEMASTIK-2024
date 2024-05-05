using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetUpCard : MonoBehaviour
{
    public ItemBaseSO itemBaseSO;
    public Button thisButton;
    public Image background;
    public Image backgroundFocus;
    public PlayerUsableGeneralData generalData
    {
        get => itemBaseSO.generalData;
    }
    public ItemType type;
    public TextMeshProUGUI textHeader;
    public static event Action<ItemBaseSO> onChoseItem;
    public void SetUpData(ItemBaseSO so, ItemType type)
    {
        this.itemBaseSO = so;
        this.type = type;
        textHeader.text = generalData.name;
        thisButton.interactable = generalData.unlocked;
        backgroundFocus.gameObject.SetActive(false);
        UpdateData();
    }
    private void Start()
    {
        DetailedCardView.OnBoughtSomething += UpdateData;
    }
    private void UpdateData()
    {
        bool isUnlocked;
        if(itemBaseSO is ConsumableItemSO)
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked = generalData.unlocked;
        }
        thisButton.interactable = isUnlocked;
        UpdateCardData(isUnlocked);   
    }
    private void UpdateColor()
    {
        background.color = generalData.unlocked ? Color.white : Color.gray;
    }
    private void UpdateCardData(bool input)
    {
        background.color = input ? Color.white : Color.gray;
    }
    public void SetPlayerData()
    {
        switch(type)
        {
            case ItemType.Item:
                ConsumableItemSO itemSO = itemBaseSO as ConsumableItemSO;
                GameManager.Instance.SetChosenItemSO(itemSO);
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
        onChoseItem?.Invoke(itemBaseSO);
    }
}
