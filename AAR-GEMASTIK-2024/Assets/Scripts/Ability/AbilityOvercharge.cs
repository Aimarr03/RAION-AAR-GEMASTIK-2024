using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOvercharge : AbilityBase
{
    public float duration;
    private bool canBeUsed = true;
    public float totalDuration
    {
        get
        {
            float multiplier = duration * 0.5f;
            return duration + (multiplier - (abilitySO.abilityData.level - 1));
        }
    }
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (!canBeUsed) return;
        StartCoroutine(OnExecute());
    }
    private IEnumerator OnExecute()
    {
        float tempCooldown = intervalCooldown;
        canBeUsed = false;
        intervalCooldown /= 2;
        Debug.Log("POWAAHHHH");
        yield return new WaitForSeconds(totalDuration);
        intervalCooldown = tempCooldown;
        Debug.Log("NO PWAHH RIP");
        StartCoroutine(OnCooldown());
    }

    public override IEnumerator OnCooldown()
    {
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(intervalCooldown);
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
}
