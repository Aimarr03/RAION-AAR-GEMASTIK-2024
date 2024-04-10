using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponSO weaponSO;
    public Transform firePointBlank;
    protected bool isCooldown;
    protected PlayerCoreSystem playerCoreSystem;
    [SerializeField] protected float interval;
    public abstract void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection);
    public abstract IEnumerator ProcessCooldown();
    public void SetPlayerCoreSystem(PlayerCoreSystem coreSystem)
    {
        this.playerCoreSystem = coreSystem;
    }
}
