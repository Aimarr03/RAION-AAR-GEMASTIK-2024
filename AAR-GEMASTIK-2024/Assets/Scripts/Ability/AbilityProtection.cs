using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProtection : AbilityBase
{
    
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        Debug.Log("Passive cannot be invoked");
    }

    public override IEnumerator OnCooldown()
    {
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(intervalCooldown);
        float currentTimer = 0;
        while (currentTimer <= intervalCooldown)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
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
