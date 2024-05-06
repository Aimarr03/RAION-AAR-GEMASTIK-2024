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
    public Image unavailableIcon;
    public Image icon;
    public PlayerUsableGeneralData generalData
    {
        get => itemBaseSO.generalData;
    }
    public ItemType type;
    public TextMeshProUGUI textHeader;
    public TextMeshProUGUI textLevel;
    public Color unavailableColor;
    public Color grayUnavailableColor;
    public static event Action<ItemBaseSO> onChoseItem;
    public void SetUpData(ItemBaseSO so, ItemType type)
    {
        this.itemBaseSO = so;
        this.type = type;
        string headerText = generalData.name;
        string levelText = "lvl " + generalData.level.ToString();
        if (itemBaseSO is ConsumableItemSO consumableItemSO)
        {
            headerText += $" {consumableItemSO.itemTier}";
            levelText = $"Qty: {consumableItemSO.quantity}";
        }
        textHeader.text = headerText;
        textLevel.text = levelText;
        if(itemBaseSO.generalData.icon != null)
        {
            icon.sprite = itemBaseSO.generalData.icon;
        }
        thisButton.interactable = generalData.unlocked;
        icon.color = generalData.unlocked ? Color.white : unavailableColor;
        textLevel.color = generalData.unlocked? textLevel.color : unavailableColor;
        textHeader.color = generalData.unlocked ? textHeader.color : unavailableColor;
        
        unavailableIcon.gameObject.SetActive(!generalData.unlocked);
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
