using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShockwave : AbilityBase
{
    [SerializeField] private float radiusShock;
    [SerializeField] private int energyUsage;
    [SerializeField] private int IncreaseEnergyCapacity, DecreaseHealthCapacity;
    private EnergySystem energySystem;
    private OxygenSystem oxygenSystem;
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (!isInvokable) return;
        if (isCooldown) return;
        if(this.playerCoreSystem == null || this.playerCoreSystem != playerCoreSystem) this.playerCoreSystem = playerCoreSystem;
        _BaseSustainabilitySystem energySystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Energy);
        energySystem.OnDecreaseValue(energyUsage);
        Collider[] targetHit = Physics.OverlapSphere(playerCoreSystem.transform.position, radiusShock);
        foreach (Collider collider in targetHit)
        {
            if(collider.gameObject.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
            {
                Debug.Log(collider.gameObject.name + " can be damaged");
            }
            Debug.Log(collider.gameObject.name);
        }
        StartCoroutine(OnCooldown());
        Debug.Log("Shockwave is Used");
    }

    public override IEnumerator OnCooldown()
    {
        float currentInterval = 0;
        while(currentInterval < intervalCooldown)
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
    private void OnDisable()
    {
        oxygenSystem.OnChangeValue -= OxygenSystem_OnChangeValue;
    }
    private void OxygenSystem_OnChangeValue(SustainabilityData obj)
    {
        energySystem.OnIncreaseValue(1);
        Debug.Log("Energy System is Increase by One");
    }
}
