using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class GameData
{
    public List<LevelData> levels;
    public bool newGame;
    public bool tutorialGameplay;
    public bool tutorialShop;
    public int money;
    public SerializableDictionary<WeaponType, WeaponSaveData> weaponData;
    public SerializableDictionary<AbilityType, AbilitySaveData> abilityData;
    public SerializableDictionary<SustainabilityType, int> sustainabilityData;
    public SerializableDictionary<ItemTier, int> healthItemData;
    public SerializableDictionary<ItemTier, int> energyItemData;
    public SerializableDictionary<ItemTier, int> oxygenItemData;

    public LevelData GetLevelData(string level)
    {
        string[] parts = level.Split(new char[] { ' ', '_' });
        Debug.Log(parts[1]);
        string levelForm = parts[1];
        string subLevel = parts[2];
        int levelIndex = int.Parse(levelForm) - 1;
        return levels[levelIndex];
    }
    public SubLevelData GetSubLevelData(string level)
    {
        string[] parts = level.Split(new char[] { ' ', '_' });
        string levelForm = parts[0];
        string subLevel = parts[1];
        int levelIndex = int.Parse(levelForm) - 1;
        int subLevelIndex = int.Parse(subLevel) - 1;
        return levels[levelIndex].subLevels[subLevelIndex];
    }
    public SubLevelData GetSubLevelData(LevelData levelData, int subLevel)
    {
        return levelData.subLevels[subLevel];
    }
    public GameData()
    {
        levels = new List<LevelData>();
        newGame = true;
        money = 100;
        for (int i = 0; i < 3; i++)
        {
            LevelData level = new LevelData();
            level.levelName = "Level " + (i+1);
            level.subLevels = new List<SubLevelData>();
            if (i == 0) level.hasBeenUnlocked = true;
            for(int  j = 0; j <3; j++)
            {
                SubLevelData subLevel = new SubLevelData();
                subLevel.subLevelName = level.levelName + "_"+ j;
                subLevel.trashList = new SerializableDictionary<string, bool>();
                subLevel.fishNeedHelpList = new SerializableDictionary<string, bool>();
                level.subLevels.Add(subLevel);
            }
            levels.Add(level);
        }
        HandleNewWeaponData();
        HandleNewAbilityData();
        HandleNewSustainabilityData();
        HandleNewItemData();
    }
    void HandleNewWeaponData()
    {
        weaponData = new SerializableDictionary<WeaponType, WeaponSaveData>();
        weaponData.Add(WeaponType.harpoon, new WeaponSaveData(1, true));
        weaponData.Add(WeaponType.torpedo, new WeaponSaveData(1, false));
        weaponData.Add(WeaponType.machinegun, new WeaponSaveData(1, false));
        weaponData.Add(WeaponType.energyblaster, new WeaponSaveData(1, false));
    }
    void HandleNewAbilityData()
    {
        abilityData = new SerializableDictionary<AbilityType, AbilitySaveData>();
        abilityData.Add(AbilityType.Dash, new AbilitySaveData(1, true));
        abilityData.Add(AbilityType.Shield, new AbilitySaveData(1, false));
        abilityData.Add(AbilityType.Shockwave, new AbilitySaveData(1, false));
        abilityData.Add(AbilityType.Overcharge, new AbilitySaveData(1, false));
    }
    void HandleNewSustainabilityData()
    {
        sustainabilityData = new SerializableDictionary<SustainabilityType, int>();
        sustainabilityData.Add(SustainabilityType.Health, 1);
        sustainabilityData.Add(SustainabilityType.Capacity, 1);
        sustainabilityData.Add(SustainabilityType.Energy, 1);
        sustainabilityData.Add(SustainabilityType.Oxygen, 1);
    }
    void HandleNewItemData()
    {
        healthItemData = new SerializableDictionary<ItemTier, int>();
        oxygenItemData = new SerializableDictionary<ItemTier, int>();
        energyItemData = new SerializableDictionary<ItemTier, int>();

        healthItemData.Add(ItemTier.C, 3);
        healthItemData.Add(ItemTier.B, 0);
        healthItemData.Add(ItemTier.A, 0);

        oxygenItemData.Add(ItemTier.C, 3);
        oxygenItemData.Add(ItemTier.B, 0);
        oxygenItemData.Add(ItemTier.A, 0);

        energyItemData.Add(ItemTier.C, 3);
        energyItemData.Add(ItemTier.B, 0);
        energyItemData.Add(ItemTier.A, 0);

    }
}
[Serializable]
public class WeaponSaveData
{
    public int level;
    public bool unlocked;
    public WeaponSaveData(int level,  bool unlocked)
    {
        this.level = level;
        this.unlocked = unlocked;
    }
}
[Serializable]
public class AbilitySaveData
{
    public int level;
    public bool unlocked;
    public AbilitySaveData(int level, bool unlocked)
    {
        this.level = level;
        this.unlocked = unlocked;
    }
}
[Serializable]
public class ItemSaveData
{
    public ItemTier tier;
    public int quantity;
    public ItemSaveData(ItemTier tier, int quantity)
    {
        this.tier = tier;
        this.quantity = quantity;
    }
}
