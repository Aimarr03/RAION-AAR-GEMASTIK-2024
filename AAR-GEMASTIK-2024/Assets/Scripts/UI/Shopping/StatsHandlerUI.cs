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
        templateCard.gameObject.SetActive(false);
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
                else
                {
                    HandleWeaponStatsUIUpgradeMode(weaponSO);
                }
                break;
            case ConsumableItemSO consumableItemSO:
                HandleConsumableItemStatsUIBuyMode(consumableItemSO);
                break;
            case AbilitySO abilitySO:
                if (ShopManager.instance.shopMode == ShopMode.Buy)
                {
                    HandleAbilityStatsUIBuyMode(abilitySO);
                }
                else
                {
                    HandleAbilityStatsUIUpgradeMode(abilitySO);
                }
                break;
            case SustainabilitySystemSO sustainabilitySystemSO:
                if (ShopManager.instance.shopMode == ShopMode.Buy)
                {
                    HandleSustainabilityStatsUIBuyMode(sustainabilitySystemSO);
                }
                else
                {
                    HandleSustainabilityStatsUIUpgradeMode(sustainabilitySystemSO);
                }
                break;
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
            bool unlocked = weaponSO.generalData.unlocked;
            statsCard.SetUpData(new StatsDataPassing(null, currentWeaponStatsType.ToString(), statsValue.ToString(), unlocked));
        }
    }
    private void HandleWeaponStatsUIUpgradeMode(WeaponSO weaponSO)
    {
        List<WeaponStats> statsList = weaponSO.statsList;
        foreach (WeaponStats currentWeaponStatsType in statsList)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            WeaponStatsType statsData = GetWeaponStatsData(currentWeaponStatsType);
            float statsValue = weaponSO.GetDataStatsBuyMode(currentWeaponStatsType);
            float[] upgradeStatsValue = weaponSO.GetDataStatsUpgradeMode(currentWeaponStatsType);
            bool unlocked = weaponSO.generalData.unlocked;
            statsCard.SetUpData(new StatsDataPassing(null, currentWeaponStatsType.ToString(), statsValue.ToString(),unlocked));
            statsCard.OnUpdateUpgradeStats(upgradeStatsValue, unlocked);
        }
    }
    private void HandleConsumableItemStatsUIBuyMode(ConsumableItemSO consumableItemSO)
    {
        SustainabilityType sustainabilityList = consumableItemSO.type;
        SustainabilityStatsType statsType = GetSustainabilityType(sustainabilityList);
        Transform newCard = Instantiate(templateCard.transform, StatsContainer);
        newCard.gameObject.SetActive(true);
        StatsCard statsCard = newCard.GetComponent<StatsCard>();
        float statsValue = consumableItemSO.GetTotalValueBasedOnTier();
        string type = "RECOVER " + consumableItemSO.type;
        bool unlocked = !consumableItemSO.generalData.unlocked;
        statsCard.OnUpdateUpgradeStats(null, unlocked);
        statsCard.SetUpData(new StatsDataPassing(null, type, statsValue.ToString(), unlocked));
    }
    private void HandleAbilityStatsUIBuyMode(AbilitySO abilitySO)
    {
        List<AbilityStats> statsList = abilitySO.statsList;
        foreach(AbilityStats currentStats in statsList)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            AbilityStatsType abilityStatsData = GetAbilityStatsData(currentStats);
            float statasValue = abilitySO.GetStatsDataBasedOnTypeBuyMode(currentStats);
            bool unlocked = abilitySO.generalData.unlocked;
            statsCard.SetUpData(new StatsDataPassing(null, currentStats.ToString(), statasValue.ToString(), unlocked));
        }
    }
    private void HandleAbilityStatsUIUpgradeMode(AbilitySO abilitySO)
    {
        List<AbilityStats> statsList = abilitySO.statsList;
        foreach (AbilityStats currentStats in statsList)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            AbilityStatsType abilityStatsData = GetAbilityStatsData(currentStats);
            float statsValue = abilitySO.GetStatsDataBasedOnTypeBuyMode(currentStats);
            float[] upgradeStatsValue = abilitySO.GetStatsDataBasedOnTypeUpgradeMode(currentStats);
            bool unlocked = abilitySO.generalData.unlocked;
            statsCard.SetUpData(new StatsDataPassing(null, currentStats.ToString(), statsValue.ToString(), unlocked));
            statsCard.OnUpdateUpgradeStats(upgradeStatsValue, unlocked);
        }
    }
    private void HandleSustainabilityStatsUIBuyMode(SustainabilitySystemSO sustainabillitySO)
    {
        SustainabilityType sustainabilityList = sustainabillitySO.sustainabilityType;
        SustainabilityStatsType statsType = GetSustainabilityType(sustainabilityList);
        Transform newCard = Instantiate(templateCard.transform, StatsContainer);
        newCard.gameObject.SetActive(true);
        StatsCard statsCard = newCard.GetComponent<StatsCard>();
        float statsValue = sustainabillitySO.maxValueTimesLevel;
        string type = "" + sustainabillitySO.sustainabilityType;
        bool unlocked = sustainabillitySO.generalData.unlocked;
        statsCard.SetUpData(new StatsDataPassing(null, type, statsValue.ToString(), unlocked));
    }
    private void HandleSustainabilityStatsUIUpgradeMode(SustainabilitySystemSO sustainabillitySO)
    {
        SustainabilityType sustainabilityList = sustainabillitySO.sustainabilityType;
        SustainabilityStatsType statsType = GetSustainabilityType(sustainabilityList);
        Transform newCard = Instantiate(templateCard.transform, StatsContainer);
        newCard.gameObject.SetActive(true);
        StatsCard statsCard = newCard.GetComponent<StatsCard>();
        float statsValue = sustainabillitySO.maxValueTimesLevel;
        float[] upgradeStatsValue = sustainabillitySO.upgradeStatsValue();
        string type = "" + sustainabillitySO.sustainabilityType;
        bool unlocked = sustainabillitySO.generalData.unlocked;
        statsCard.SetUpData(new StatsDataPassing(null, type, statsValue.ToString(), unlocked));
        statsCard.OnUpdateUpgradeStats(upgradeStatsValue, unlocked);
    }
    private SustainabilityStatsType GetSustainabilityType(SustainabilityType sustainabilityStatsType)
    {
        foreach (SustainabilityStatsType currentSustainabilityStatsType in sustainabilityListStats)
        {
            if (currentSustainabilityStatsType.type == sustainabilityStatsType) return currentSustainabilityStatsType;
        }
        return sustainabilityListStats[0];
    }

    private WeaponStatsType GetWeaponStatsData(WeaponStats weaponStats)
    {
        foreach(WeaponStatsType statsType in weaponStatsListStats)
        {
            if(statsType.type == weaponStats) return statsType;
        }
        return weaponStatsListStats[0];
    }
    private AbilityStatsType GetAbilityStatsData(AbilityStats abilityStats)
    {
        foreach (AbilityStatsType statsType in abilityStatsListStats)
        {
            if (statsType.type == abilityStats) return statsType;
        }
        return abilityStatsListStats[0];
    }
}
