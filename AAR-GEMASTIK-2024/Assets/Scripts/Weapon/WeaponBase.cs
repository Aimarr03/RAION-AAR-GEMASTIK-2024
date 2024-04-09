using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponSO weaponSO;
    public Transform firePointBlank;
    protected bool isCooldown;
    [SerializeField] protected float interval;
    public abstract void Fire(PlayerWeaponSystem coreSystem);
    public abstract IEnumerator ProcessCooldown();
}
