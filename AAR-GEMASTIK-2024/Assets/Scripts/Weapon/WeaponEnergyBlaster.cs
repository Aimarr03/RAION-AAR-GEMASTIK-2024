using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnergyBlaster : WeaponBase
{
    [SerializeField] private float maxDurationCharge;
    private float currentChargeDuration;
    private bool isOnRightDirection;
    private bool isFullyCharge;
    private Coroutine chargingWeaponCoroutine;
    private void OnEnable()
    {
        PlayerInputSystem.OnReleasedInvokeWeaponUsage += PlayerInputSystem_OnReleasedInvokeWeaponUsage;
    }
    private void OnDisable()
    {
        PlayerInputSystem.OnReleasedInvokeWeaponUsage -= PlayerInputSystem_OnReleasedInvokeWeaponUsage;
    }

    private void PlayerInputSystem_OnReleasedInvokeWeaponUsage()
    {
        StopCoroutine(chargingWeaponCoroutine);
        StopCoroutine(ChargingWeapon());
        BaseBullet baseBullet = objectPooling.UnloadBullet();
        
        WeaponBulletData weaponBulletData = weaponSO.bulletData;
        weaponBulletData.damage = (weaponBulletData.damage * GetPercentageFloat());
        weaponBulletData.isFullyCharge = isFullyCharge;
        Debug.Log("Weapon charged " + weaponBulletData.isFullyCharge);

        baseBullet.SetUpBullet(weaponBulletData, isOnRightDirection, playerCoreSystem.transform.rotation);
        StartCoroutine(ProcessCooldown());
        isFullyCharge = false;
    }

    public override void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection)
    {
        if (isCooldown) return;
        isFullyCharge = false;
        this.isOnRightDirection = isOnRightDirection;
        chargingWeaponCoroutine = StartCoroutine(ChargingWeapon());
    }
    private IEnumerator ChargingWeapon()
    {
        Debug.Log("Charging Weapon");
        currentChargeDuration = 0;
        while(currentChargeDuration < maxDurationCharge)
        {
            currentChargeDuration += Time.deltaTime;
            yield return null;
        }
        isFullyCharge = true;
        Debug.Log("Fully Charge");
    }
    public override IEnumerator ProcessCooldown()
    {
        playerCoreSystem.weaponSystem.TriggerDoneFire(interval);
        Debug.Log("Is Cooldown");
        isCooldown = true;
        float currentInterval = 0;
        while (currentInterval <= interval)
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done");
    }
    public float GetPercentageFloat()
    {
        return currentChargeDuration / maxDurationCharge;
    }
}
