using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityDataSO
{
    public int level;
}
[CreateAssetMenu(fileName ="New Ability SO", menuName = "Ability/Create New Ability SO")]
public class AbilitySO : ScriptableObject
{
    public PlayerUsableGeneralData generalData;
    public AbilityDataSO abilityData;
    public Transform prefab;
}
