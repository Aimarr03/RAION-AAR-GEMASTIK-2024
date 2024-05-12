using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct PlayerUsableGeneralData
{
    public string name;
    [TextArea(4, 10)]
    public string description;
    public Sprite icon;
    public int level;
    public int buyPrice;
    public int upgradePrice;
    public bool unlocked;
    public ItemType itemType;
    public int totalUpgradePrice
    {
        get => upgradePrice + ((level - 1) * upgradePrice);
    }
}
public class ItemBaseSO : ScriptableObject, IComparable<ItemBaseSO>
{
    public PlayerUsableGeneralData generalData;

    public int CompareTo(ItemBaseSO other)
    {
        return UpgradeModeComparison(other);
    }
    public int UpgradeModeComparison(ItemBaseSO other)
    {
        int boolComparison = generalData.unlocked.CompareTo(other.generalData.unlocked);
        if (boolComparison != 0)
        {
            return -boolComparison;
        }
        else
        {
            return generalData.name.CompareTo(other.generalData.name);
        }
    }
    public int BuyModeComparison(ItemBaseSO other)
    {
        int boolComparison = generalData.unlocked.CompareTo(other.generalData.unlocked);
        if (boolComparison != 0)
        {
            return boolComparison;
        }
        else
        {
            return generalData.name.CompareTo(other.generalData.name);
        }
    }
}
