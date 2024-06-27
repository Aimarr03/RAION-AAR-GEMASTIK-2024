using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOvercharge : AbilityBase
{
    [SerializeField] private ParticleSystem effect;
    public float duration;
    public float GetMultiplierDuration(int level)
    {
        float maxDuration = duration + ((level - 1) * (duration * 0.075f));
        return maxDuration;
    }
    public float GetMultiplierCooldown(int level)
    {
        float maxCooldown = intervalCooldown - ((level - 1) * (intervalCooldown * 0.03f));
        return maxCooldown;
    }
    private bool canBeUsed = true;
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (!canBeUsed) return;
        StartCoroutine(OnExecute());
    }
    private IEnumerator OnExecute()
    {
        effect.Play();
        float tempCooldown = playerCoreSystem.weaponSystem.GetWeaponSO().GetWeapon.interval;
        canBeUsed = false;
        playerCoreSystem.weaponSystem.GetWeaponSO().GetWeapon.interval = tempCooldown / 2;
        Debug.Log("POWAAHHHH " + playerCoreSystem.weaponSystem.GetWeaponSO().GetWeapon.interval);
        yield return new WaitForSeconds(GetMultiplierCooldown(level));
        playerCoreSystem.weaponSystem.GetWeaponSO().GetWeapon.interval = tempCooldown;
        effect.Stop();
        Debug.Log("NO PWAHH RIP " + playerCoreSystem.weaponSystem.GetWeaponSO().GetWeapon.interval);
        StartCoroutine(OnCooldown());
    }

    public override IEnumerator OnCooldown()
    {
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(GetMultiplierCooldown(level));
        float currentInterval = 0;
        while (currentInterval < intervalCooldown)
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        canBeUsed = true;
        Debug.Log("Overcharge can be used again");
    }

    public override AbilityType GetAbilityType() => AbilityType.Overcharge;

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>
        {
            new BuyStats("Max Duration", GetMultiplierDuration(level).ToString("0.0")),
            new BuyStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0")),
            new BuyStats("Multiplier", "2x")
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>
        {
            new UpgradeStats("Max Duration", GetMultiplierDuration(level).ToString("0.0"), GetMultiplierDuration(level + 1).ToString("0.0")),
            new UpgradeStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0"), GetMultiplierCooldown(level+1).ToString("0.0")),
            new UpgradeStats("Multiplier", "2x", null)
        };
    }
}
