using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityDataSO
{
    public int level;
}
[CreateAssetMenu(fileName ="New Ability SO", menuName = "Ability/Create New Ability SO")]
public class AbilitySO : ScriptableObject, IBuyable, IUpgradable
{
    [Header("General Data")]
    public PlayerUsableGeneralData generalData;
    [Header("Ability Data")]
    public AbilityDataSO abilityData;
    public Transform prefab;
    public bool isInvokable;
    public float cooldownDuration;

    public void Buy()
    {
        throw new System.NotImplementedException();
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
