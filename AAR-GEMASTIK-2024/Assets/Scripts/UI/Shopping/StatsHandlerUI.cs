using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct SustainabilityStatsType
{
    public SustainabilityType type;
    public Sprite Icon;
}
[Serializable]
public struct WeaponStatsType
{
    public WeaponStats type;
    public Sprite Icon;
}
[Serializable]
public struct AbilityStatsType
{
    public AbilityStats type;
    public Sprite Icon;
}
public enum WeaponStats
{
    damage,
    cooldown,
    firerate

}
public enum AbilityStats
{
    damage,
    cooldown,
    duration
}
public class StatsHandlerUI : MonoBehaviour
{
    [Header("List of Stats Data")]
    public List<SustainabilityStatsType> sustainabilityListStats;
    public List<AbilityStatsType> abilityStatsListStats;
    public List<WeaponStatsType> weaponStatsListStats;

    [Header("Other")]
    public DetailedCardView detailedCardView;
    public Transform StatsContainer;
    public StatsCard templateCard;

    private void Awake()
    {
        detailedCardView = GetComponent<DetailedCardView>();
    }
    public void OnSetUpStats(ItemBaseSO itemBaseSO)
    {
        foreach(Transform currentCard in StatsContainer)
        {
            if (currentCard == templateCard.transform) continue;
            Destroy(currentCard);
        }
        switch (itemBaseSO)
        {
            case WeaponSO weaponSO: break;
            case ConsumableItemSO consumableItemSO: break;
            case AbilitySO abilitySO: break;
            case SustainabilitySystemSO sustainabilitySystemSO: break;
        }
    }
    private void HandleWeaponStatsUI(WeaponSO weaponSO)
    {
        
    }
}
