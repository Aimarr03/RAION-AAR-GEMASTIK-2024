using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier
{
    A, B, C
}
[CreateAssetMenu(fileName ="New Consumable Items", menuName ="Consumable Item/Create New Consumable Item SO")]
public class ConsumableItemSO : ScriptableObject, IBuyable, IUpgradable, IQuantifiable
{
    [Header("General Data")]
    public PlayerUsableGeneralData generalData;
    [Header("Consumable Item Data")]
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

    public void Buy()
    {
        Debug.Log("Attempt to Buy " + generalData.name);
    }
    public void OnUse()
    {
        throw new System.NotImplementedException();
    }

    public void OnBuy()
    {
        throw new System.NotImplementedException();
    }

    public void Upgrade()
    {
        Debug.Log("Attempt to Upgrade " + generalData.name);
    }
}
