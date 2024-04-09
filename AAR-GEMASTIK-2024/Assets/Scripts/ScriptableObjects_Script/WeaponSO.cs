using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeaponGeneralData
{
    public string name;
    public string description;
    public Sprite icon;
}
[Serializable]
public struct WeaponBulletData
{
    public int level;
    public float speed;
    public float damage;
}

[CreateAssetMenu(fileName ="New Weapon SO", menuName ="Weapon/Create New Weapon SO")]
public class WeaponSO : ScriptableObject
{
    public WeaponGeneralData weaponData;
    public WeaponBulletData bulletData;
    public Transform weapon;
    public Transform bullet;
}
