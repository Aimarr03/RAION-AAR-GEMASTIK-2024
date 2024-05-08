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
        for(int index = 0; index< StatsContainer.childCount; index++)
        {
            Transform currentCard = StatsContainer.GetChild(index);
            if (currentCard == templateCard.transform) continue;
            Destroy(currentCard.gameObject);
        }
        switch (itemBaseSO)
        {
            case WeaponSO weaponSO: 
                if(ShopManager.instance.shopMode == ShopMode.Buy)
                {
                    HandleWeaponStatsUIBuyMode(weaponSO);
                }
                break;
            case ConsumableItemSO consumableItemSO: break;
            case AbilitySO abilitySO: break;
            case SustainabilitySystemSO sustainabilitySystemSO: break;
        }
    }
    private void HandleWeaponStatsUIBuyMode(WeaponSO weaponSO)
    {
        List<WeaponStats> statsList = weaponSO.statsList;
        foreach (WeaponStats currentWeaponStatsType in statsList)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            WeaponStatsType statsData = GetWeaponStatsData(currentWeaponStatsType);
            float statsValue = weaponSO.GetDataStatsBuyMode(currentWeaponStatsType);
            statsCard.SetUpData(new StatsDataPassing(null, currentWeaponStatsType.ToString(), statsValue.ToString()));
        }
    }
    private WeaponStatsType GetWeaponStatsData(WeaponStats weaponStats)
    {
        foreach(WeaponStatsType statsType in weaponStatsListStats)
        {
            if(statsType.type == weaponStats) return statsType;
        }
        return weaponStatsListStats[0];
    }
}
