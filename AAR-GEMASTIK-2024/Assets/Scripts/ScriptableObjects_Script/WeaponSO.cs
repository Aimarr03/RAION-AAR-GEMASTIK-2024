using System;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    harpoon,
    torpedo,
    machinegun,
    energyblaster
}
[Serializable]
public struct WeaponBulletData
{
    public int level;
    public float speed;
    public float damage;
    public bool isFullyCharge;
    public int totalDamage
    {
        get
        {
            int multiplierDamageBasedLevel = (int)damage / 2;
            return (int)(damage + (level -1 * multiplierDamageBasedLevel));
        }
    }
    public int nextLevelTotalDamage
    {
        get
        {
            int multiplierDamageBasedLevel = (int)damage / 2;
            return (int)(damage + (level * multiplierDamageBasedLevel));
        }
    }
    
}

[CreateAssetMenu(fileName ="New Weapon SO", menuName ="Weapon/Create New Weapon SO")]
public class WeaponSO : ItemBaseSO, IBuyable,IUpgradable
{
    public List<WeaponStats> statsList;
    public WeaponBulletData bulletData;
    public bool unlocked;
    public float cooldownBetweenFiringBullet;
    public Transform weapon;
    public Transform bullet;
    public int ammountToHold;
    public WeaponType type;
    public float GetCooldownBetweenFiringBullet
    {
        get
        {
            float multiplierShotCooldown = cooldownBetweenFiringBullet / 20;
            return cooldownBetweenFiringBullet - ((generalData.level - 1) * multiplierShotCooldown);
        }
    }

    public float GetCooldownBetweenFiringBulletNextLevel
    {
        get
        {
            float multiplierShotCooldown = cooldownBetweenFiringBullet / 20;
            return cooldownBetweenFiringBullet - (generalData.level * multiplierShotCooldown);
        }
    }

    public void Buy()
    {
        Debug.Log("Attempt to Buy " + generalData.name);
    }

    public void Upgrade()
    {
        Debug.Log("Attempt to Upgrade " + generalData.name);
    }
    public float GetDataStatsBuyMode(WeaponStats type)
    {
        switch (type)
        {
            case WeaponStats.damage:
                return bulletData.totalDamage;
            case WeaponStats.firerate:
                return GetCooldownBetweenFiringBullet;
            default: return 0;
        }
    }
    public float[] GetDataStatsUpgradeMode(WeaponStats type)
    {
        float[] data = new float[2];
        switch (type)
        {
            case WeaponStats.damage:
                data[0] = bulletData.totalDamage;
                data[1] = bulletData.nextLevelTotalDamage;
                break;
            case WeaponStats.firerate:
                data[0] = bulletData.totalDamage;
                data[1] = bulletData.nextLevelTotalDamage;
                break;
        }
        return data;
    }
}
