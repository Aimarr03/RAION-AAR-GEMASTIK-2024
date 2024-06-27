using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProtection : AbilityBase
{
    [SerializeField] private AudioClip unInterractable;
    [SerializeField] private Transform effect;
    public float GetMultiplierCooldown(int level)
    {
        float maxCooldown = intervalCooldown - ((level -1)* (intervalCooldown * 0.05f));
        return maxCooldown;
    }
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        Debug.Log("Passive cannot be invoked");
        AudioManager.Instance?.PlaySFX(unInterractable);
    }
    private void Start()
    {
        StartEffect();
    }
    private void StartEffect()
    {
        effect.DORotate(new Vector3(0, 360, 0), 3.3f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }
    public override AbilityType GetAbilityType() => AbilityType.Shield;

    public override List<BuyStats> GetBuyStats()
    {
        return new List<BuyStats>
        {
            new BuyStats("Cooldown", GetMultiplierCooldown(level).ToString("0.0"))
        };
    }

    public override List<UpgradeStats> GetUpgradeStats()
    {
        return new List<UpgradeStats>
        {
            new UpgradeStats("Cooldown", GetMultiplierCooldown(level).ToString(),GetMultiplierCooldown(level+1).ToString("0.0"))
        };
    }
    public override IEnumerator OnCooldown()
    {
        effect.gameObject.SetActive(false);
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(intervalCooldown);
        float currentTimer = 0;
        while (currentTimer <= GetMultiplierCooldown(level))
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        effect.gameObject.SetActive(true);
        StartEffect();
        isCooldown = false;
        playerCoreSystem.canBlock = true;
        Debug.Log("Protection Is UP");
    }

    public override void SetPlayerCoreSystem(PlayerCoreSystem playerCoreSystem)
    {
        base.SetPlayerCoreSystem(playerCoreSystem);
        playerCoreSystem.canBlock = true;
        this.playerCoreSystem.OnBlocking += PlayerCoreSystem_OnBlocking;
    }
    private void OnDestroy()
    {
        playerCoreSystem.OnBlocking -= PlayerCoreSystem_OnBlocking;
    }

    private void PlayerCoreSystem_OnBlocking()
    {
        Debug.Log("Block things");
        playerCoreSystem.canBlock = false;
        StartCoroutine(OnCooldown());
    }
}
