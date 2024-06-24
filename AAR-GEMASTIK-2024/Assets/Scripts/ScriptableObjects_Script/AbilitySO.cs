using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Dash,
    Shockwave,
    Overcharge,
    Shield
}
public struct AbilityDataSO
{
    public int level;
}
[CreateAssetMenu(fileName ="New Ability SO", menuName = "Ability/Create New Ability SO")]
public class AbilitySO : ItemBaseSO, IBuyable, IUpgradable, IDataPersistance
{
    public AbilityDataSO abilityData;
    public Transform prefab;
    public bool isInvokable;
    public float cooldownDuration;
    public AbilityBase GetAbility => prefab.GetComponent<AbilityBase>();
    
    
    public void Buy()
    {
        
    }

    public void Upgrade()
    {
        
    }

    public void LoadScene(GameData gameData)
    {
        Debug.Log("Load Ability Data");
        AbilitySaveData data =  gameData.abilityData[GetAbility.GetAbilityType()];
        generalData.level = data.level;
        generalData.unlocked = data.unlocked;
    }

    public void SaveScene(ref GameData gameData)
    {
        Debug.Log("Save Ability Data");
        AbilitySaveData data = gameData.abilityData[GetAbility.GetAbilityType()];
        data.level = generalData.level;
        data.unlocked = generalData.unlocked;
    }
}
