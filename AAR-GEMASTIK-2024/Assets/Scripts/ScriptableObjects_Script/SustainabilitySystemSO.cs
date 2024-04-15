using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Sustainability SO", menuName ="Sustainability System/Create NEW Sustainability System SO")]
public class SustainabilitySystemSO : ItemBaseSO, IUpgradable
{
    public int maxValue;
    public SustainabilityType sustainabilityType;

    public int maxLevelTimesLevel
    {
        get => maxValue + GetMultiplierLevelValue();
    }
    public int GetMultiplierLevelValue() => (int)((maxValue * 0.2f) * (generalData.level - 1));

    public void Upgrade()
    {
        
    }
}
