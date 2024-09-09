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
    public bool isUnlocked;

    public float progress;
    public float fishNeededHelpProgress = 0;
    public int fishNeededHelpCountDone = 0;
    public float trashProgress = 0;
    public int trashCountDone = 0;
    public SerializableDictionary<string, bool> trashList;
    public SerializableDictionary<string, bool> fishNeedHelpList;
    public SerializableDictionary<string, bool> conversationList;
    public SerializableDictionary<string, SerializableDictionary<string, bool>> additionalCollectableObjects;
    public SerializableDictionary<string, int> collectedAdditionalCollectableObjects;
}