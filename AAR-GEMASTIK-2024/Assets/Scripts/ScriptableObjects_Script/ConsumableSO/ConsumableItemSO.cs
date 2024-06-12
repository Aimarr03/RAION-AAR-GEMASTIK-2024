using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier
{
    A, B, C
}

public abstract class ConsumableItemSO : ItemBaseSO, IBuyable, IUpgradable, IQuantifiable, IDataPersistance
{
    public abstract SustainabilityType type { get; }
    public ItemTier itemTier;
    public int value;
    public int quantity;

    public void UseItem(PlayerCoreSystem system)
    {
        if (type == SustainabilityType.Capacity) return;
        int totalValue = GetTotalValueBasedOnTier();
        _BaseSustainabilitySystem baseSystem = system.GetSustainabilitySystem(type);
        baseSystem.OnIncreaseValue(totalValue);
    }
    public int GetTotalValueBasedOnTier()
    {
        int totalValue = 0;
        switch (itemTier)
        {
            case ItemTier.A:
                totalValue = (int)(value * 1f);
                break;
            case ItemTier.B:
                totalValue = (int)(value * 1.4f);
                break;
            case ItemTier.C:
                totalValue = (int)(value * 1.6f);
                break;
        }
        return totalValue;
    }

    public void Buy()
    {
        Debug.Log("Attempt to Buy " + generalData.name);
    }
    public void OnUse()
    {
        
    }

    public void OnBuy()
    {
        
    }

    public void Upgrade()
    {
        Debug.Log("Attempt to Upgrade " + generalData.name);
    }

    public void LoadScene(GameData gameData)
    {
        Debug.Log("Load Item Data");
        switch (type)
        {
            case SustainabilityType.Health:
                quantity = gameData.healthItemData[itemTier];
                break;
            case SustainabilityType.Energy:
                quantity = gameData.energyItemData[itemTier];
                break;
            case SustainabilityType.Oxygen:
                quantity = gameData.oxygenItemData[itemTier];
                break;
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        Debug.Log("Save Item Data");
        switch (type)
        {
            case SustainabilityType.Health:
                gameData.healthItemData[itemTier] = quantity;
                break;
            case SustainabilityType.Energy:
                gameData.energyItemData[itemTier] = quantity;
                break;
            case SustainabilityType.Oxygen:
                gameData.oxygenItemData[itemTier] = quantity;
                break;
        }
    }
}
