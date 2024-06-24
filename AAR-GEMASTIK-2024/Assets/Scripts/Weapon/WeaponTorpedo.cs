using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTorpedo : WeaponBase
{
    [SerializeField] private int baseDamage = 80;
    [SerializeField] private float baseRadius = 10;

    public int GetMultiplierDamage(int level)
    {
        float maxDamage = baseDamage + ((level-1) * (baseDamage * 0.33f));
        return (int)maxDamage;
    }
    public float GetMultiplierRadius(int level)
    {
        float maxRadius = baseRadius + ((level - 1) * (baseRadius * 0.12f));
        return (float)maxRadius;
    }
    public float GetMultiplierCooldown(int level)
    {
        float maxCooldown = interval - ((level - 1) * (interval * 0.09f));
        return maxCooldown;
    }
    public override void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection)
    {
        if (isCooldown) return;
        Debug.Log("Torpedo Weapon is firing");
        
        BaseBullet baseBullet = LoadBullet();
        baseBullet.SetUpBullet(isOnRightDirection, playerCoreSystem.transform.rotation);
        StartCoroutine(ProcessCooldown());
    }

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>()
        {
            new BuyStats("Damage", GetMultiplierDamage(level).ToString()),
            new BuyStats("Radius", GetMultiplierRadius(level).ToString("0.0")),
            new BuyStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0")),
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>()
        {
            new UpgradeStats("Damage", GetMultiplierDamage(level).ToString(),GetMultiplierDamage(level+1).ToString()),
            new UpgradeStats("Radius", GetMultiplierRadius(level).ToString("0.0"),GetMultiplierRadius(level+1).ToString("0.0")),
            new UpgradeStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0"), GetMultiplierCooldown(level+1).ToString("0.0")),
        };
    }

    public override WeaponType GetWeaponType() => WeaponType.torpedo;

    public override IEnumerator ProcessCooldown()
    {
        playerCoreSystem.weaponSystem.TriggerDoneFire(GetMultiplierCooldown(level));
        Debug.Log("Is Cooldown");
        isCooldown = true;
        float currentInterval = 0;
        while (currentInterval <= GetMultiplierCooldown(level))
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done");
    }
}
