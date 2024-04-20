using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagerGameplayUI : MonoBehaviour
{
    [SerializeField] List<ItemGameplayUI> gameplayUIList;
    [SerializeField] PlayerConsumptionItemSystem consumptionSystem;

    private void Awake()
    {
        SustainabilityType type = consumptionSystem.GetSustainabilityTypeBasedOnIndex();
        foreach(ItemGameplayUI item in gameplayUIList)
        {
            Debug.Log(item.type);
            ConsumableItemSO itemSO = consumptionSystem.GetConsumableItemSO(item.type);
            item.SetUp(itemSO);
            if(item.type == type) item.OnFocusIcon();
        }
    }
    private void Start()
    {
        consumptionSystem.onUseItem += ConsumptionSystem_onUseItem;
        consumptionSystem.onChangeItem += ConsumptionSystem_onChangeItem;
    }

    private void ConsumptionSystem_onUseItem(SustainabilityType type, int quantity, float cooldown)
    {
        foreach(ItemGameplayUI item in gameplayUIList)
        {
            item.StartCooldown(cooldown);
            if(item.type == type) item.quantity.text = quantity.ToString();
        }
    }

    private void ConsumptionSystem_onChangeItem(SustainabilityType type, float cooldownDuration)
    {
        Debug.Log("Chagen Item on UI");
        foreach(ItemGameplayUI item in gameplayUIList)
        {
            item.OnNotFocusedIcon();
            item.StartCooldown(cooldownDuration);
            if (item.type == type) item.OnFocusIcon();
        }
    }
}
