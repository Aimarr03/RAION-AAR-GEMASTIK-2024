using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHarpoon : WeaponBase
{
    [SerializeField] private int baseDamage = 30;
    [SerializeField] private float speed = 12;

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
            new BuyStats("Damage", GetMultiplierDamage(level).ToString()),
            new BuyStats("Speed", GetMultiplierSpeed(level).ToString()),
            new BuyStats("Cooldown", GetMultiplierInterval(level).ToString())
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>()
        {
            new UpgradeStats("Damage", GetMultiplierDamage(level).ToString(), GetMultiplierDamage(level+1).ToString()),
            new UpgradeStats("Speed", GetMultiplierSpeed(level).ToString(),GetMultiplierSpeed(level+1).ToString()),
            new UpgradeStats("Cooldown", GetMultiplierInterval(level).ToString(),GetMultiplierInterval(level + 1).ToString())
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
