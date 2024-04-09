using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Ability SO", menuName = "Ability/Create New Ability SO")]
public class AbilitySO : ScriptableObject
{
    public WeaponGeneralData generalData;
    public Transform prefab;
}
