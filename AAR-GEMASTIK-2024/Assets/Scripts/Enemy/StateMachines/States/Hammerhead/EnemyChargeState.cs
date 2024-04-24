using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class EnemyChargeState : EnemyBaseState
{
    private float chargeSpeed;
    private float maxDuration = 3f;
    private bool isCharging;
    private Vector3 direction;

    private EnemyBaseState nextState;
    public EnemyChargeState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, float chargeSpeed) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.chargeSpeed = chargeSpeed;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        Debug.Log("Enter Charge State");
        isCharging = true;
        direction = (playerCoreSystem.transform.position - enemy.transform.position).normalized;
        OnCharging();
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        if (!isCharging) return;
        enemy.transform.position += direction * chargeSpeed * Time.deltaTime;
        if(Vector2.Distance(playerCoreSystem.transform.position, enemy.transform.position) > 40) isCharging=false;
        
    }
    private async void OnCharging()
    {
        await Task.Delay((int)maxDuration * 1000);
        isCharging=false;
        enemyStateMachine.OnTransitionState(nextState);
    }
}
