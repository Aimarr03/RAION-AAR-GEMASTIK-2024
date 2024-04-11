using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponSO weaponSO;
    public Transform firePointBlank;
    protected bool isCooldown;
    protected PlayerCoreSystem playerCoreSystem;
    protected ObjectPooling objectPooling;
    [SerializeField] protected float interval;
    
    public virtual void Awake()
    {
        objectPooling = GetComponentInChildren<ObjectPooling>();
    }
    public abstract void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection);
    public abstract IEnumerator ProcessCooldown();
    public void SetPlayerCoreSystem(PlayerCoreSystem coreSystem)
    {
        this.playerCoreSystem = coreSystem;
    }
    public void SetObjectPooling(WeaponSO weaponSO)
    {
        objectPooling.InitializePool(weaponSO);
    }
    public BaseBullet LoadBullet()
    {
        BaseBullet baseBullet = objectPooling.UnloadBullet();
        baseBullet.transform.position = firePointBlank.position;
        baseBullet.transform.rotation = Quaternion.identity;
        return baseBullet;
    }
}
