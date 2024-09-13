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
    private bool isUnlocked;
    public PlayerUsableGeneralData generalData
    {
        get => itemBaseSO.generalData;
    }
    public ItemType type;
    public TextMeshProUGUI textHeader;
    public TextMeshProUGUI textLevel;
    public Color defaultColor;
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
        Debug.Log($"{itemBaseSO} + {generalData.unlocked}");
        defaultColor = textLevel.color;
        icon.color = generalData.unlocked ? Color.white : unavailableColor;
        textLevel.color = generalData.unlocked? defaultColor : unavailableColor;
        textHeader.color = generalData.unlocked ? defaultColor : unavailableColor;
        
        unavailableIcon.gameObject.SetActive(!generalData.unlocked);
        backgroundFocus.gameObject.SetActive(false);
        UpdateData();
        DetailedCardView.OnBoughtSomething += UpdateData;
        DetailedCardView.OnUpgradedSomething += UpdateData;
    }

    private void OnDisable()
    {
        DetailedCardView.OnBoughtSomething -= UpdateData;
        DetailedCardView.OnUpgradedSomething -= UpdateData;
    }
    private void UpdateData()
    {
        //Debug.Log("Update Data " + itemBaseSO.name);
        bool isUnlocked;
        if(itemBaseSO is ConsumableItemSO)
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked = generalData.unlocked;
            unavailableIcon.gameObject.SetActive(!isUnlocked);
            textHeader.gameObject.SetActive(isUnlocked);
            textLevel.gameObject.SetActive(isUnlocked);
            textLevel.text = "lvl " + generalData.level;
            icon.color = generalData.unlocked ? Color.white : unavailableColor;
            textLevel.color = generalData.unlocked ? defaultColor : unavailableColor;
            textHeader.color = generalData.unlocked ? defaultColor : unavailableColor;
        }
        this.isUnlocked = isUnlocked;
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
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.interractable);
        onChoseItem?.Invoke(itemBaseSO);
    }
}
