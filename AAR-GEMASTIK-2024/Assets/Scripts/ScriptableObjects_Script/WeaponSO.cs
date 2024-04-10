using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerUsableGeneralData
{
    public string name;
    [TextArea(4,10)]
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
    public PlayerUsableGeneralData weaponData;
    public WeaponBulletData bulletData;
    public Transform weapon;
    public Transform bullet;
}
