using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier
{
    A, B, C
}
[CreateAssetMenu(fileName ="New Consumable Items", menuName ="Consumable Item/Create New Consumable Item SO")]
public class ConsumableItemSO : ScriptableObject
{
    public PlayerUsableGeneralData generalData;
    public SustainabilityType type;
    public ItemTier itemTier;
    public int value;

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
}
