using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Sustainability SO", menuName ="Sustainability System/Create NEW Sustainability System SO")]
public class SustainabilitySystemSO : ItemBaseSO, IUpgradable, IDataPersistance
{
    public int maxValue;
    public float multiplierValue;
    public SustainabilityType sustainabilityType;

    public int maxValueTimesLevel
    {
        get => maxValue + GetMultiplierLevelValue();
    }
    public int maxValueTimesNextLevel
    {
        get => maxValue + (int)((maxValue * multiplierValue) * generalData.level);
    }
    
    public int GetMultiplierLevelValue() => (int)((maxValue * multiplierValue) * (generalData.level - 1));
    

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
    public List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>
        {
            new UpgradeStats($"Type", $"{sustainabilityType}", null),
            new UpgradeStats($"Max Value", $"{maxValueTimesLevel}", $"{maxValueTimesNextLevel}"),
        };
    }
    public List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats> 
        {
            new BuyStats($"Type", $"{sustainabilityType}"),
            new BuyStats($"Max Value", $"{maxValueTimesLevel}"),
        };
    }
}
