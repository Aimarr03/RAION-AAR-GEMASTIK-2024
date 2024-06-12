using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Sustainability SO", menuName ="Sustainability System/Create NEW Sustainability System SO")]
public class SustainabilitySystemSO : ItemBaseSO, IUpgradable, IDataPersistance
{
    public int maxValue;
    public SustainabilityType sustainabilityType;

    public int maxValueTimesLevel
    {
        get => maxValue + GetMultiplierLevelValue();
    }
    public int maxValueTimesNextLevel
    {
        get => maxValue + (int)((maxValue * 0.2f) * generalData.level);
    }
    public float[] upgradeStatsValue()
    {
        float currentValue = maxValueTimesLevel;
        float nextLevelValue = maxValueTimesNextLevel;
        return new float[]
        {
            currentValue, nextLevelValue
        };
    }
    public int GetMultiplierLevelValue() => (int)((maxValue * 0.2f) * (generalData.level - 1));
    

    public void Upgrade()
    {
        
    }

    public void LoadScene(GameData gameData)
    {
        Debug.Log("Loading Sustainability Data");
        int level= gameData.sustainabilityData[sustainabilityType];
        generalData.level = level;
    }

    public void SaveScene(ref GameData gameData)
    {
        Debug.Log("Saving Sustainability Data");
        gameData.sustainabilityData[sustainabilityType] = generalData.level;
    }
}
