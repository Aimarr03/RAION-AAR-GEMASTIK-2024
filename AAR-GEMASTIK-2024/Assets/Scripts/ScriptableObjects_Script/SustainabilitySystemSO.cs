using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Sustainability SO", menuName ="Sustainability System/Create NEW Sustainability System SO")]
public class SustainabilitySystemSO : ScriptableObject
{
    public int maxHealth;
    public int level;
    public SustainabilityType sustainabilityType;

    public int maxLevelTimesLevel
    {
        get => maxHealth + (int)((maxHealth / 2) * level-1);
    }
}
