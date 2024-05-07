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
    public void Buy()
    {
        
    }

    public void Upgrade()
    {
        
    }
}
