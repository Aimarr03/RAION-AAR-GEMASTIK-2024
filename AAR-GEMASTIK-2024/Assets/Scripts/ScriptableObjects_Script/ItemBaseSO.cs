using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct PlayerUsableGeneralData
{
    public string name;
    [TextArea(4, 10)]
    public string description;
    public Sprite icon;
    public int level;
    public int buyPrice;
    public int upgradePrice;
    public bool unlocked;
}
public class ItemBaseSO : ScriptableObject
{
    public PlayerUsableGeneralData generalData;
}
