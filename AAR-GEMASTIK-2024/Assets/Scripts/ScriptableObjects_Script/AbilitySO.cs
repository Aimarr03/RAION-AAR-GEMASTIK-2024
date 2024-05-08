using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityDataSO
{
    public int level;
}
[CreateAssetMenu(fileName ="New Ability SO", menuName = "Ability/Create New Ability SO")]
public class AbilitySO : ItemBaseSO, IBuyable, IUpgradable
{
    public List<AbilityStats> statsList;
    public AbilityDataSO abilityData;
    public Transform prefab;
    public bool isInvokable;
    public float cooldownDuration;
    public AbilityBase abilityLogic
    {
        get
        {
            if (prefab == null) return null;
            return prefab.GetComponent<AbilityBase>();
        }
    }
    public float GetStatsDataBasedOnTypeBuyMode(AbilityStats statsType)
    {
        switch (statsType)
        {
            case AbilityStats.duration:
                float duration = prefab.GetComponent<AbilityOvercharge>().duration;
                return GetMultiplierStatsBasedOnLevel(duration);
            case AbilityStats.cooldown:
                return GetMultiplierStatsBasedOnLevel(cooldownDuration);
            case AbilityStats.damage:
                float damage = prefab.GetComponent<AbilityShockwave>().damage;
                return GetMultiplierStatsBasedOnLevel(damage);
            default: return 0;
        }
    }
    public float[] GetStatsDataBasedOnTypeUpgradeMode(AbilityStats statsType)
    {
        switch (statsType)
        {
            case AbilityStats.duration:
                float duration = prefab.GetComponent<AbilityOvercharge>().duration;
                return GetUpgradeStatsValue(duration);
            case AbilityStats.cooldown:
                return GetUpgradeStatsValue(cooldownDuration);
            case AbilityStats.damage:
                float damage = prefab.GetComponent<AbilityShockwave>().damage;
                return GetUpgradeStatsValue(damage);
            default: return null;
        }
    }
    public float GetMultiplierStatsBasedOnLevel(float value)
    {
        return value + ((value * 0.2f) * (generalData.level - 1));
    }
    public float GetMultiplierStatsBasedOnNextLevel(float value)
    {
        return value + ((value * 0.2f) * (generalData.level));
    }
    public float[] GetUpgradeStatsValue(float value)
    {
        float currentLevelValue = GetMultiplierStatsBasedOnLevel(value);
        float nextLevelValue = GetMultiplierStatsBasedOnNextLevel(value);
        return new float[] { currentLevelValue, nextLevelValue };
    }
    public void Buy()
    {
        
    }

    public void Upgrade()
    {
        
    }
}
