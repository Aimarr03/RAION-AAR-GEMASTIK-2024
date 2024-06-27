using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShockwave : AbilityBase
{
    [SerializeField] private float radiusShock = 75f;
    [SerializeField] private float stunDuration = 1.3f;
    [SerializeField] private int energyUsage = 20;
    [SerializeField] private int baseDamage = 30;
    [SerializeField] private int IncreaseEnergyCapacity, DecreaseHealthCapacity;
    [SerializeField] private ParticleSystem ElectricShockEffect;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, GetMultiplierRadiusShock(level));
    }
    public float GetMultiplierRadiusShock(int level)
    {
        float maxRadiusShock = radiusShock + ((level-1) * (radiusShock * 0.02f));
        return maxRadiusShock;
    }
    public float GetMultiplierStunDuration(int level)
    {
        float maxDurationStun = stunDuration + ((level - 1) * (stunDuration * 0.05f));
        return maxDurationStun;
    }
    public int GetMultiplierEnergyUsage(int level)
    {
        float maxUsage = energyUsage + ((level - 1) * (energyUsage * 0.2f));
        return (int) maxUsage;
    }
    public float GetMultiplierCooldown(int level)
    {
        float maxCooldown = intervalCooldown - ((level - 1) * (intervalCooldown * 0.06f));
        return maxCooldown;
    }
    private EnergySystem energySystem;
    private OxygenSystem oxygenSystem;

    public int damage;
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (!isInvokable) return;
        if (isCooldown) return;
        if(this.playerCoreSystem == null || this.playerCoreSystem != playerCoreSystem) this.playerCoreSystem = playerCoreSystem;
        _BaseSustainabilitySystem energySystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Energy);
        energySystem.OnDecreaseValue(GetMultiplierEnergyUsage(level));
        Collider[] targetHit = Physics.OverlapSphere(playerCoreSystem.transform.position, GetMultiplierRadiusShock(level));
        ElectricShockEffect.Play();
        foreach (Collider collider in targetHit)
        {
            if(collider.gameObject.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
            {
                if (collider.gameObject.TryGetComponent(out PlayerCoreSystem coreSystem)) continue;
                damagableUnit.TakeDamage(baseDamage);
                damagableUnit.OnDisableMove(GetMultiplierStunDuration(level), 20);
            }
            Debug.Log(collider.gameObject.name);
        }
        StartCoroutine(OnCooldown());
        Debug.Log("Shockwave is Used");
    }

    public override IEnumerator OnCooldown()
    {
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(GetMultiplierCooldown(level));
        float currentInterval = 0;
        while(currentInterval < GetMultiplierCooldown(level))
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Shockwave can be used again");
    }

    public override void SetPlayerCoreSystem(PlayerCoreSystem playerCoreSystem)
    {
        base.SetPlayerCoreSystem(playerCoreSystem);
        energySystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Energy) as EnergySystem;
        energySystem.OnAddMaxValue(IncreaseEnergyCapacity);
        Debug.Log("Increase energy capacity by " + IncreaseEnergyCapacity);
        HealthSystem healthSystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Health) as HealthSystem;
        healthSystem.OnAddMaxValue(DecreaseHealthCapacity);
        Debug.Log("Decrease health capacity by " + DecreaseHealthCapacity);
        oxygenSystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Oxygen) as OxygenSystem;
        oxygenSystem.OnChangeValue += OxygenSystem_OnChangeValue;
    }
    private void OnDestroy()
    {
        oxygenSystem.OnChangeValue -= OxygenSystem_OnChangeValue;
    }
    private void OxygenSystem_OnChangeValue(SustainabilityData obj)
    {
        energySystem.OnIncreaseValue(1);
        Debug.Log("Energy System is Increase by One");
    }
    public override AbilityType GetAbilityType() => AbilityType.Shockwave;

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>{
            new BuyStats("Damage", damage.ToString()),
            new BuyStats("Radius Shock", GetMultiplierRadiusShock(level).ToString("0.0")),
            new BuyStats("Energy Usage", GetMultiplierEnergyUsage(level).ToString()),
            new BuyStats("Stun", GetMultiplierStunDuration(level).ToString("0.0")),
            new BuyStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0")),
            new BuyStats("Decreased Health", DecreaseHealthCapacity.ToString()),
            new BuyStats("Increase Energy", IncreaseEnergyCapacity.ToString()),
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>{
            new UpgradeStats("Damage", damage.ToString(), null),
            new UpgradeStats("Radius Shock", GetMultiplierRadiusShock(level).ToString("0.0"), GetMultiplierRadiusShock(level+1).ToString("0.0")),
            new UpgradeStats("Energy Usage", GetMultiplierEnergyUsage(level).ToString(), GetMultiplierEnergyUsage(level + 1).ToString()),
            new UpgradeStats("Stun", GetMultiplierStunDuration(level).ToString("0.0"), GetMultiplierStunDuration(level + 1).ToString("0.0")),
            new UpgradeStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0"), GetMultiplierCooldown(level + 1).ToString("0.0")),
            new UpgradeStats("Decreased Health", DecreaseHealthCapacity.ToString(), null),
            new UpgradeStats("Increase Energy", IncreaseEnergyCapacity.ToString(), null),
        };
    }
}
