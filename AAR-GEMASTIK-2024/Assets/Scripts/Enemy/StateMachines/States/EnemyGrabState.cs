using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyGrabState : EnemyBaseState
{
    private float jumpForce = 25f;
    private bool hasGrabbed;
    private EnemyBaseState nextState;
    public EnemyGrabState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask) : base(enemyStateMachine, enemy, playerLayerMask)
    {
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        Debug.Log("Attemp to Grab Player");
        hasGrabbed = false;
        playerCoreSystem.OnBreakingFree += PlayerCoreSystem_OnBreakingFree;
        Vector3 direction = (playerCoreSystem.transform.position -  enemy.transform.position).normalized;
        enemy.rigidBody.AddForce(direction * jumpForce, ForceMode.Impulse);
        OnAttemptGrabPlayer();
    }

    private void PlayerCoreSystem_OnBreakingFree()
    {
        Debug.Log("Player is Free");
        OnReleasedPlayerFromGrabbing();
        playerCoreSystem.OnBreakingFree -= PlayerCoreSystem_OnBreakingFree;
        playerCoreSystem = null;
    }
    private async void OnReleasedPlayerFromGrabbing()
    {
        Debug.Log("Releasing");
        enemy.rigidBody.AddForce(Vector3.up * 3, ForceMode.Force);
        await Task.Delay(1000);
        Debug.Log("Resting");
        enemy.rigidBody.velocity = Vector3.zero;
        await Task.Delay(2500);
        enemyStateMachine.OnTransitionState(nextState);
    }
    private async void OnAttemptGrabPlayer()
    {
        await Task.Delay(3000);
        if (hasGrabbed) return;
        Debug.Log("Grab attempt failed");
        playerCoreSystem = null;
        Debug.Log("Resting");
        enemy.rigidBody.velocity = Vector3.zero;
        await Task.Delay(2500);
        enemyStateMachine.OnTransitionState(nextState);
    }

    public override void OnExitState()
    {
        hasGrabbed = false;
        playerCoreSystem = null;
    }

    public override void OnUpdateState()
    {
        if (playerCoreSystem == null) return;
        if (!hasGrabbed)
        {
            CheckDistance();
        }
        else
        {
            OnGrabTakeDamage();
        }
    }
    private void CheckDistance()
    {
        if(Vector3.Distance(enemy.transform.position, playerCoreSystem.transform.position) < 2)
        {
            Debug.Log("Grab Player");
            enemy.rigidBody.velocity = Vector3.zero;
            playerCoreSystem.transform.position = enemy.transform.position;
            playerCoreSystem.OnDisableMove(12, 40);
            hasGrabbed = true;
        }
    }
    private void OnGrabTakeDamage()
    {
        playerCoreSystem.TakeDamage(enemy.damage);
    }

}
