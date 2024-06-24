using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
[Serializable]
public struct SustainabilityStatsType
{
    public SustainabilityType type;
    public Sprite Icon;
}
public class StatsHandlerUI : MonoBehaviour
{
    [Header("List of Stats Data")]
    public List<SustainabilityStatsType> sustainabilityListStats;

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
        List<BuyStats> statsList = weaponSO.GetWeapon.GetBuyStats();
        foreach (BuyStats currentWeaponBuyStats in statsList)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            bool unlocked = weaponSO.generalData.unlocked;
            statsCard.SetUpData(currentWeaponBuyStats);
        }
    }
    private void HandleWeaponStatsUIUpgradeMode(WeaponSO weaponSO)
    {
        List<UpgradeStats> upgradeStatsList = weaponSO.GetWeapon.GetUpgradeStats();
        List<BuyStats> buyStatsList = weaponSO.GetWeapon.GetBuyStats();
        for(int index = 0; index < upgradeStatsList.Count; index++)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            bool unlocked = weaponSO.generalData.unlocked;
            statsCard.SetUpData(buyStatsList[index]);
            statsCard.OnUpdateUpgradeStats(upgradeStatsList[index], unlocked);
        }
    }
    private void HandleConsumableItemStatsUIBuyMode(ConsumableItemSO consumableItemSO)
    {
        foreach (BuyStats currentWeaponBuyStats in consumableItemSO.GetBuyStats())
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            bool unlocked = !consumableItemSO.generalData.unlocked;
            statsCard.SetUpData(currentWeaponBuyStats);
        }
    }
    private void HandleAbilityStatsUIBuyMode(AbilitySO abilitySO)
    {
        List<BuyStats> statsList = abilitySO.GetAbility.GetBuyStats();
        foreach(BuyStats currentStats in statsList)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            statsCard.SetUpData(currentStats);
        }
    }
    private void HandleAbilityStatsUIUpgradeMode(AbilitySO abilitySO)
    {
        List<BuyStats> buyStatsList = abilitySO.GetAbility.GetBuyStats();
        List<UpgradeStats> upgradeStatsList = abilitySO.GetAbility.GetUpgradeStats();
        for (int index = 0; index < upgradeStatsList.Count; index++)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            bool unlocked = abilitySO.generalData.unlocked;
            statsCard.SetUpData(buyStatsList[index]);
            statsCard.OnUpdateUpgradeStats(upgradeStatsList[index], unlocked);
        }
    }
    private void HandleSustainabilityStatsUIBuyMode(SustainabilitySystemSO sustainabillitySO)
    {
        List<BuyStats> buyStatsList = sustainabillitySO.GetBuyStats();
        foreach(BuyStats currentStats in  buyStatsList)
        {
            Transform newCard = Instantiate(templateCard.transform, StatsContainer);
            newCard.gameObject.SetActive(true);
            StatsCard statsCard = newCard.GetComponent<StatsCard>();
            bool unlocked = sustainabillitySO.generalData.unlocked;
            statsCard.SetUpData(currentStats);
        }
    }
    private void HandleSustainabilityStatsUIUpgradeMode(SustainabilitySystemSO sustainabillitySO)
    {
        List<BuyStats> buyStatsList = sustainabillitySO.GetBuyStats();
        List<UpgradeStats> upgradeStatsList = sustainabillitySO.GetUpgradeStats();
        for (int index = 0; index < upgradeStatsList.Count; index++)
        {
            Transform newCardStats = Instantiate(templateCard.transform, StatsContainer);
            newCardStats.gameObject.SetActive(true);
            StatsCard statsCard = newCardStats.GetComponent<StatsCard>();
            bool unlocked = sustainabillitySO.generalData.unlocked;
            statsCard.SetUpData(buyStatsList[index]);
            statsCard.OnUpdateUpgradeStats(upgradeStatsList[index], unlocked);
        }
    }
}
