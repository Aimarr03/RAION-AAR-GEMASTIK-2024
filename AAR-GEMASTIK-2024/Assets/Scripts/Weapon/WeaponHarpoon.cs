using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHarpoon : WeaponBase
{
    [SerializeField] private int baseDamage = 75;
    [SerializeField] private float speed = 12;
    [SerializeField] private float baseSlow = 0.33f;
    [SerializeField] private float baseSlowDuration = 2f;
    public int GetMultiplierDamage(int level) 
    {
        float totalDamage = baseDamage + ((level-1) * (baseDamage * 0.65f));
        return (int) totalDamage;
    }
    public float GetMultiplierSpeed(int level)
    {
        float totalSpeed = speed + ((level - 1) * (baseDamage * 0.2f));
        return (float) totalSpeed;
    }
    public float GetMultiplierInterval(int level)
    {
        float totalCooldown = interval - ((level - 1) * (interval * 0.1f));
        return (float) totalCooldown;
    }
    public float GetMultiplierSlow(int level)
    {
        float totalSlow = baseSlow + ((level - 1) * (0.05f));
        return (float)totalSlow;
    }
    public float GetMultiplierSlowDuration(int level)
    {
        float totalSlow = baseSlowDuration + ((level - 1) * (0.4f));
        return (float)totalSlow;
    }
    public override void Fire(PlayerWeaponSystem weaponSystem, bool isOnRightDirection)
    {
        if (isCooldown) return;
        Debug.Log("Harpoon Weapon is firing");

        BulletHarpoon baseBullet = LoadBullet() as BulletHarpoon;
        baseBullet.SetUpBullet(isOnRightDirection, playerCoreSystem.transform.rotation);
        StartCoroutine(ProcessCooldown());
    }

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>()
        {
            new BuyStats("Damage", GetMultiplierDamage(level).ToString("0.0")),
            new BuyStats("Speed", GetMultiplierSpeed(level).ToString("0.0")),
            new BuyStats("Cooldown", GetMultiplierInterval(level).ToString("0.0")),
            new BuyStats("Slow", GetMultiplierSlow(level).ToString("0.0")),
            new BuyStats("Slow Duration", GetMultiplierSlowDuration(level).ToString("0.0"))
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>()
        {
            new UpgradeStats("Damage", GetMultiplierDamage(level).ToString("0.0"), GetMultiplierDamage(level+1).ToString("0.0")),
            new UpgradeStats("Speed", GetMultiplierSpeed(level).ToString("0.0"),GetMultiplierSpeed(level+1).ToString("0.0")),
            new UpgradeStats("Cooldown", GetMultiplierInterval(level).ToString("0.0"),GetMultiplierInterval(level + 1).ToString("0.0")),
            new UpgradeStats("Slow", GetMultiplierSlow(level).ToString("0.0"), GetMultiplierSlow(level +1).ToString("0.0")),
            new UpgradeStats ("Slow Duration", GetMultiplierSlowDuration(level).ToString("0.0"), GetMultiplierSlowDuration(level + 1).ToString("0.0"))
        };
    }

    public override WeaponType GetWeaponType() => WeaponType.harpoon;

    public override IEnumerator ProcessCooldown()
    {
        Debug.Log("Is Cooldown");
        isCooldown = true;
        playerCoreSystem.weaponSystem.TriggerDoneFire(GetMultiplierInterval(level));
        float currentInterval = 0;
        while(currentInterval <= GetMultiplierInterval(level))
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done");
    }
}
