using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponSO weaponSO;
    public int level;
    public Transform firePointBlank;
    protected bool isCooldown;
    protected PlayerCoreSystem playerCoreSystem;
    protected ObjectPooling objectPooling;
    public float interval;
    
    public virtual void Awake()
    {
        level = 0;
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
        baseBullet.weaponBase = this;
        baseBullet.transform.parent = null;
        baseBullet.transform.position = firePointBlank.position;
        baseBullet.transform.rotation = Quaternion.identity;
        AudioManager.Instance.PlaySFX(baseBullet.OnCreated);
        return baseBullet;
    }
    public void SetUpData()
    {
        level = weaponSO.generalData.level;
    }
    public abstract WeaponType GetWeaponType();

    public abstract List<UpgradeStats> GetUpgradeStats();
    public abstract List<BuyStats> GetBuyStats();
}


