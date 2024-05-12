using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyInstantGrabState : EnemyBaseState
{
    private EnemyBaseState nextState;
    private bool hasGrabbed;
    private int damage;
    public EnemyInstantGrabState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, int damage) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.damage = damage;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    public override void OnDrawGizmos()
    {

    }

    public override void OnEnterState()
    {
        hasGrabbed = true;
        playerCoreSystem.OnDisableMove(6, 35);
        playerCoreSystem.OnBreakingFree += PlayerCoreSystem_OnBreakingFree;
    }

    private void PlayerCoreSystem_OnBreakingFree()
    {
        hasGrabbed = false;
        OnReleasePlayer();
        playerCoreSystem.OnBreakingFree -= PlayerCoreSystem_OnBreakingFree;
    }
    private async void OnReleasePlayer()
    {
        enemy.rigidBody.velocity = Vector3.zero;
        Debug.Log("Resting");
        await Task.Delay(2000);
        nextState.SetPlayerCoreSystem(playerCoreSystem);
        enemyStateMachine.OnTransitionState(nextState);
    }

    public override void OnExitState()
    {
        Debug.Log("Start hunting again");
    }

    public override void OnUpdateState()
    {
        if (hasGrabbed) DealDamage();
    }
    private void DealDamage() 
    {
        playerCoreSystem.transform.position = enemy.transform.position;
        playerCoreSystem.TakeDamage(damage); 
    }

    
}
