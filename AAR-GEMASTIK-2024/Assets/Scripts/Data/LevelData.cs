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
    public bool hasBeenExpediction;
    public float progress;
    public float fishProgress;
    public float trashProgress;
    public SerializableDictionary<string, bool> trashList;
    public SerializableDictionary<string, bool> fishList;
}
