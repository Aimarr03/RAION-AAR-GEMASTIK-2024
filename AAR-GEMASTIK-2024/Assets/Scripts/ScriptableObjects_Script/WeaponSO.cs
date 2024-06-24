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
[CreateAssetMenu(fileName ="New Weapon SO", menuName ="Weapon/Create New Weapon SO")]
public class WeaponSO : ItemBaseSO, IBuyable,IUpgradable, IDataPersistance
{
    public Transform weapon;
    public Transform bullet;
    public int ammountToHold;
    public WeaponBase GetWeapon => weapon.GetComponent<WeaponBase>();

    public BaseBullet GetBullet => bullet.GetComponent<BaseBullet>();
    public void Buy()
    {
        Debug.Log("Attempt to Buy " + generalData.name);
    }

    public void Upgrade()
    {
        Debug.Log("Attempt to Upgrade " + generalData.name);
    }
    

    public void LoadScene(GameData gameData)
    {
        WeaponSaveData data = gameData.weaponData[GetWeapon.GetWeaponType()];
        Debug.Log(data);
        Debug.Log($"Unlocked = {data.unlocked}, level {data.level}");
        generalData.level = data.level;
        generalData.unlocked = data.unlocked;
    }

    public void SaveScene(ref GameData gameData)
    {
        Debug.Log("Saving Game Data For Weapon");
        WeaponSaveData data = gameData.weaponData[GetWeapon.GetWeaponType()];
        data.level = generalData.level;
        data.unlocked = generalData.unlocked;
    }
}
