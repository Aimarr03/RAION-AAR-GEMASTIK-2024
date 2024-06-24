using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnergyBlaster : WeaponBase
{
    [SerializeField] private float maxDurationCharge = 1.3f;
    [SerializeField] private int baseDamage = 65;
    [SerializeField] private float baseRadius = 7.5f;
    [SerializeField] private float stunDuration = 0.4f;
    
    public int GetMultiplierDamage(int level)
    {
        float maxDamage = baseDamage + ((level - 1) * (baseDamage * 0.75f));
        return (int)maxDamage;
    }
    public float GetMultiplierRadius(int level)
    {
        float maxRadius = baseRadius + ((level - 1) * (baseRadius * 0.1f));
        return maxRadius;
    }
    public float GetMultiplierMaxDurationCharge(int level)
    {
        float maxDurationChargeMultiplier = maxDurationCharge - ((level - 1) * (maxDurationCharge * 0.07f));
        return maxDurationChargeMultiplier;
    }
    public float GetMultiplierStunDuration(int level)
    {
        float totalStunDuration = stunDuration + ((level - 1) * (stunDuration * 0.25f));
        return totalStunDuration;
    }
    public float GetMultiplierInterval(int level)
    {
        float totalInterval = interval - ((level - 1) * (interval * 0.07f));
        return totalInterval;
    }
    
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


        baseBullet.SetUpBullet(isOnRightDirection, playerCoreSystem.transform.rotation);
        StartCoroutine(ProcessCooldown());
        isFullyCharge = false;
        currentChargeDuration = 0;
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
        while(currentChargeDuration < GetMultiplierMaxDurationCharge(level))
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
        while (currentInterval <= GetMultiplierMaxDurationCharge(level))
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done");
    }
    public float GetPercentageFloat()
    {
        return currentChargeDuration / GetMultiplierMaxDurationCharge(level);
    }

    public override WeaponType GetWeaponType() => WeaponType.energyblaster;

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>()
        {
            new UpgradeStats("Damage", GetMultiplierDamage(level).ToString(), GetMultiplierDamage(level + 1).ToString()),
            new UpgradeStats("Radius", GetMultiplierRadius(level).ToString(), GetMultiplierRadius(level + 1).ToString()),
            new UpgradeStats("Duration To Max Charge", GetMultiplierMaxDurationCharge(level).ToString(), GetMultiplierMaxDurationCharge(level + 1).ToString()),
            new UpgradeStats("Stun Duration", GetMultiplierStunDuration(level).ToString(), GetMultiplierStunDuration(level + 1).ToString()),
            new UpgradeStats("Cooldown", GetMultiplierInterval(level).ToString(), GetMultiplierInterval(level+1).ToString())
        };
    }

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>()
        {
            new BuyStats("Damage", GetMultiplierDamage(level).ToString()),
            new BuyStats("Radius", GetMultiplierRadius(level).ToString()),
            new BuyStats("Duration To Max Charge", GetMultiplierMaxDurationCharge(level).ToString()),
            new BuyStats("Stun Duration", GetMultiplierStunDuration(level).ToString()),
            new BuyStats("Cooldown", GetMultiplierInterval(level).ToString())
        };
    }
    public bool GetIsFullyCharge() => isFullyCharge;
}
