using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponSO weaponSO;
    public Transform firePointBlank;
    protected bool isCooldown;
    protected float interval;
    public abstract void Fire(PlayerWeaponSystem coreSystem);
    public abstract IEnumerator ProcessCooldown();
}
