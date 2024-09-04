using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string levelName;
    public bool hasBeenUnlocked;
    public int subLevelCount;
    public int GetBanyakLangkah()
    {
        int langkah = 0;
        foreach(SubLevelData subLevelData in subLevels)
        {
            if(subLevelData.isDone) langkah++;
        }
        return langkah;
    }
    public List<SubLevelData> subLevels;
}
[Serializable]
public class SubLevelData
{
    public string subLevelName;
    public int currentCompletedPhase = 0;
    public int maxPhase = 0;
    public bool isDone;

    public float progress;
    public float fishNeededHelpProgress;
    public int fishNeededHelpCountDone;
    public float trashProgress;
    public int trashCountDone;
    public SerializableDictionary<string, bool> trashList;
    public SerializableDictionary<string, bool> fishNeedHelpList;
}
[CreateAssetMenu(fileName ="New Sub Level Name")]
public class SubLevelDescription : ScriptableObject
{
    public string subLevelName;
    public string description;
}