using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class AbilityDash : AbilityBase
{
    [SerializeField] private float ForceDash;
    private PlayerCoreSystem playerCoreSystem;
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (isCooldown) return;
        if(this.playerCoreSystem != playerCoreSystem)
        {
            this.playerCoreSystem = playerCoreSystem;
        }
        PlayerMoveSystem playerMoveSystem = playerCoreSystem.moveSystem;

        playerMoveSystem.AddSuddenForce(ForceDash);

        isCooldown = true;
        Debug.Log("Dash is Used");
    }

    public override IEnumerator OnCooldown()
    {
        float currentTimer = 0;
        while(currentTimer <= intervalCooldown)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Dash can be Used AGAIN");
    }
}
