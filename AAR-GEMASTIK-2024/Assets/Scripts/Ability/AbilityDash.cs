using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityDash : AbilityBase
{
    [SerializeField] private float ForceDash;
    [SerializeField] private ParticleSystem effect;
    public float GetMultiplierForceDash(int level)
    {
        float MaxForceDash = ForceDash + ((level - 1) * (ForceDash * 0.1f));
        return MaxForceDash;
    }
    public float GetMultiplierCooldown(int level)
    {
        float MaxCooldown = intervalCooldown - ((level - 1) * (intervalCooldown * 0.08f));
        return MaxCooldown;
    }
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (!isInvokable) return;
        if (isCooldown) return;
        if(this.playerCoreSystem != playerCoreSystem || this.playerCoreSystem == null)
        {
            this.playerCoreSystem = playerCoreSystem;
        }
        effect.Play();
        PlayerMoveSystem playerMoveSystem = playerCoreSystem.moveSystem;
        playerMoveSystem.AddSuddenForce(GetMultiplierForceDash(level));
        isCooldown = true;
        AudioManager.Instance.PlaySFX(AudioOnInvoke);
        DisableEnablePlayerMovementSystem();
        StartCoroutine(OnCooldown());
        Debug.Log("Dash is Used");
    }
    private async void DisableEnablePlayerMovementSystem()
    {
        playerCoreSystem.moveSystem.SetCanBeUsed(false);
        await Task.Delay(300);
        playerCoreSystem.moveSystem.SetCanBeUsed(true);
    }
    public override IEnumerator OnCooldown()
    {
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(GetMultiplierCooldown(level));
        float currentTimer = 0;
        while(currentTimer <= GetMultiplierCooldown(level))
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        AudioManager.Instance.PlaySFX(OnCooldownDone);
        Debug.Log("Dash can be Used AGAIN");
    }

    public override AbilityType GetAbilityType() => AbilityType.Dash;

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>
        {
            new BuyStats("Dash Power:", GetMultiplierForceDash(level).ToString("0.0")),
            new BuyStats("Cooldown: ", GetMultiplierForceDash(level +1).ToString("0.0"))
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>
        {
            new UpgradeStats("Dash Power:", GetMultiplierForceDash(level).ToString("0.0"), GetMultiplierForceDash(level+1).ToString("0.0")),
            new UpgradeStats("Cooldown: ", GetMultiplierForceDash(level).ToString("0.0"), GetMultiplierForceDash(level +1).ToString("0.0"))
        };
    }
}
