using System;
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
            return (int)(damage * (level * multiplierDamageBasedLevel));
        }
    }
}

[CreateAssetMenu(fileName ="New Weapon SO", menuName ="Weapon/Create New Weapon SO")]
public class WeaponSO : ItemBaseSO, IBuyable,IUpgradable
{
    public WeaponBulletData bulletData;
    public bool unlocked;
    public float cooldownBetweenFiringBullet;
    public Transform weapon;
    public Transform bullet;
    public int ammountToHold;
    public WeaponType type;

    public void Buy()
    {
        Debug.Log("Attempt to Buy " + generalData.name);
    }

    public void Upgrade()
    {
        Debug.Log("Attempt to Upgrade " + generalData.name);
    }
}
