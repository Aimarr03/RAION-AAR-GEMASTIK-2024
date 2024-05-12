using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityDash : AbilityBase
{
    [SerializeField] private float ForceDash;
    public override void Fire(PlayerCoreSystem playerCoreSystem)
    {
        if (!isInvokable) return;
        if (isCooldown) return;
        if(this.playerCoreSystem != playerCoreSystem || this.playerCoreSystem == null)
        {
            this.playerCoreSystem = playerCoreSystem;
        }
        PlayerMoveSystem playerMoveSystem = playerCoreSystem.moveSystem;
        playerMoveSystem.AddSuddenForce(ForceDash);
        isCooldown = true;
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
        playerCoreSystem.abilitySystem.TriggerDoneInvokingAbility(intervalCooldown);
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
