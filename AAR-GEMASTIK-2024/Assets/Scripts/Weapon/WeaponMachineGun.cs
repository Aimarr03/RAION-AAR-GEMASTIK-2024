using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMachineGun : WeaponBase
{
    
    private bool isFiring;
    private bool isOnRightDirection;
    
    private float durationToPrepareAttack;
    private Coroutine onShooting;
    private Coroutine onCooldown;
    [Header("Base Data")]
    [SerializeField] private float intervalBetweenAttack = 0.2f;
    [SerializeField] private float baseMaxDurationToPrepare = 1.3f;
    [SerializeField] private float baseDamage = 10;
    [SerializeField] private int maxBullet = 5;

    public float GetMultiplierDamage(int level)
    {
        float totalDamage = baseDamage + ((level - 1) * (baseDamage * 0.75f));
        return totalDamage;
    }
    public float GetMultiplierMaxDurationToPrepare(int level)
    {
        float totalDurationToPrepare = baseMaxDurationToPrepare - ((level - 1) * (baseMaxDurationToPrepare * 0.15f));
        return totalDurationToPrepare;
    }
    public int GetMultiplierMaxBullet(int level)
    {
        float totalBullet = maxBullet + ((level - 1) * 0.25f);
        return (int) totalBullet;
    }
    public float GetMultiplierInterval(int level)
    {
        float totalCooldown = interval - ((level - 1) * (interval * 0.2f));
        return (float)totalCooldown;
    }
    private void OnEnable()
    {
        PlayerInputSystem.OnReleasedInvokeWeaponUsage += PlayerInputSystem_OnReleasedInvokeWeaponUsage;
    }

    private void PlayerInputSystem_OnReleasedInvokeWeaponUsage()
    {
        Debug.Log("Cancel Action");
        isFiring = false;
        if (onShooting != null) StopCoroutine(onShooting);
        if(onCooldown == null) StartCoroutine(ProcessCooldown());
    }

    public override void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection)
    {
        if (isCooldown) return;
        this.isOnRightDirection = isOnRightDirection;
        isFiring = true;
        durationToPrepareAttack = 0f;
    }
    private IEnumerator OnShootBullet()
    {
        int currentBullet = 0;
        while (currentBullet < maxBullet)
        {
            BaseBullet baseBullet = LoadBullet();
            baseBullet.SetUpBullet(isOnRightDirection, playerCoreSystem.transform.rotation);
            yield return new WaitForSeconds(0.15f);
        }
        if (onCooldown == null) onCooldown = StartCoroutine(ProcessCooldown());
        /*float totalDuration = 0f;
        float currentInterval = 0f;
        while (isFiring)
        {
            currentInterval += Time.deltaTime;
            totalDuration += Time.deltaTime;
            if (totalDuration > baseMaxDurationToPrepare)
            {
                isFiring = false;
                break;
            }
            if (currentInterval >= interval)
            {
                currentInterval = 0f;
                Debug.Log("FIRE BULLET");
                BaseBullet baseBullet = LoadBullet();
                Debug.Log(baseBullet.gameObject.name);
                baseBullet.SetUpBullet(isOnRightDirection, playerCoreSystem.transform.rotation);
            }
            yield return null;
        }
        if(onCooldown == null) onCooldown = StartCoroutine(OnCooldown());*/
    }
    private void Update()
    {
        if (isCooldown) return;
        if (isFiring)
        {
            durationToPrepareAttack += Time.deltaTime;
            if(durationToPrepareAttack > baseMaxDurationToPrepare)
            {
                onShooting = StartCoroutine(OnShootBullet());
                isFiring = false;
            }
        }
    }

    public override IEnumerator ProcessCooldown()
    {
        Debug.Log("Is Cooldown");
        isCooldown = true;
        playerCoreSystem.weaponSystem.TriggerDoneFire(GetMultiplierInterval(level));
        float currentInterval = 0;
        while (currentInterval <= GetMultiplierInterval(level))
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done"); ;
    }

    public override WeaponType GetWeaponType() => WeaponType.machinegun;

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>()
        {
            new UpgradeStats("Damage", GetMultiplierDamage(level).ToString(), GetMultiplierDamage(level+1).ToString()),
            new UpgradeStats("Duration To Prepare", GetMultiplierMaxDurationToPrepare(level).ToString(),GetMultiplierMaxDurationToPrepare(level + 1).ToString()),
            new UpgradeStats("Cooldown", GetMultiplierInterval(level).ToString(),GetMultiplierInterval(level+1).ToString()),
            new UpgradeStats("Max Bullets", GetMultiplierMaxBullet(level).ToString(),GetMultiplierMaxBullet(level+1).ToString())
        };
    }

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>()
        {
            new BuyStats("Damage", GetMultiplierDamage(level).ToString()),
            new BuyStats("Duration To Prepare", GetMultiplierMaxDurationToPrepare(level).ToString()),
            new BuyStats("Cooldown", GetMultiplierInterval(level).ToString()),
            new BuyStats("Max Bullets", GetMultiplierMaxBullet(level).ToString())
        };
    }
}
