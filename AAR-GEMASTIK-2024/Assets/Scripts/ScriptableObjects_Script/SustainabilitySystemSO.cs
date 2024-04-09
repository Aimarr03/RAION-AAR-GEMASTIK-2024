using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Sustainability SO", menuName ="Sustainability System/Create NEW Sustainability System SO")]
public class SustainabilitySystemSO : ScriptableObject
{
    public int maxValue;
    public int level;
    public SustainabilityType sustainabilityType;

    public int maxLevelTimesLevel
    {
        get => maxValue + GetMultiplierLevelValue();
    }
    public int GetMultiplierLevelValue() => (int)((maxValue * 0.2f) * (level - 1));
}
