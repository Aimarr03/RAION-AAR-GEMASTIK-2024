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
    [SerializeField] private float baseDamage = 12;
    [SerializeField] private int maxBullet = 7;
    [SerializeField] private float baseSlow = 0.12f;
    [SerializeField] private float slowDuration = 1.75f;
    [SerializeField] private float stunDuration = 1.5f;

    public int GetMultiplierDamage(int level)
    {
        float totalDamage = baseDamage + ((level - 1) * (baseDamage * 0.75f));
        return (int)totalDamage;
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
    public float GetMultiplierSlow(int level)
    {
        float totalSlow = baseSlow + ((level - 1) * 0.09f);
        return totalSlow;
    }
    public float GetMultiplierSlowDuration(int level)
    {
        float totalSlowDuration = slowDuration + ((level - 1) * 0.15f);
        return totalSlowDuration;
    }
    public float GetMultilperStunDuration(int level)
    {
        float totalStunDuration = stunDuration + ((level - 1) * 0.1f);
        return totalStunDuration;
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
            currentBullet++;
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
            new UpgradeStats("Damage", GetMultiplierDamage(level).ToString("0.0"), GetMultiplierDamage(level+1).ToString("0.0")),
            new UpgradeStats("Duration To Prepare", GetMultiplierMaxDurationToPrepare(level).ToString("0.0"),GetMultiplierMaxDurationToPrepare(level + 1).ToString("0.0")),
            new UpgradeStats("Cooldown", GetMultiplierInterval(level).ToString("0.0"),GetMultiplierInterval(level+1).ToString("0.0")),
            new UpgradeStats("Max Bullets", GetMultiplierMaxBullet(level).ToString("0.0"),GetMultiplierMaxBullet(level+1).ToString("0.0")),
            new UpgradeStats("Stun Duration", GetMultilperStunDuration(level).ToString("0.0"), GetMultilperStunDuration(level+1).ToString("0.0")),
            new UpgradeStats("Slow Duration", GetMultiplierSlowDuration(level).ToString("0.0"), GetMultiplierSlowDuration((level+1)).ToString("0.0")),
            new UpgradeStats("Slow", GetMultiplierSlow(level).ToString("0.0"), GetMultiplierSlow(level+1).ToString("0.0"))
        };
    }

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>()
        {
            new BuyStats("Damage", GetMultiplierDamage(level).ToString("0.0")),
            new BuyStats("Duration To Prepare", GetMultiplierMaxDurationToPrepare(level).ToString("0.0")),
            new BuyStats("Cooldown", GetMultiplierInterval(level).ToString("0.0")),
            new BuyStats("Max Bullets", GetMultiplierMaxBullet(level).ToString("0.0")),
            new BuyStats("Stun Duration", GetMultilperStunDuration(level).ToString("0.0")),
            new BuyStats("Slow Duration", GetMultiplierSlowDuration(level).ToString("0.0")),
            new BuyStats("Slow", GetMultiplierSlow(level).ToString("0.0"))
        };
    }
}
