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
    private float maxDurationBeforeCharging = 1.2f;
    private float maxDurationCharging = 3f;
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
        
        OnCharging();
    }

    public override void OnExitState()
    {
        BeforeCharging();
    }

    public override void OnUpdateState()
    {
        if (!isCharging) return;
        enemy.transform.position += direction * chargeSpeed * Time.deltaTime;
        if(Vector2.Distance(playerCoreSystem.transform.position, enemy.transform.position) > 40) isCharging=false;
    }
    private async void OnCharging()
    {
        await Task.Delay((int)(maxDurationCharging * 1000));
        isCharging= false;
        enemyStateMachine.OnTransitionState(nextState);
    }
    private async void BeforeCharging()
    {
        await Task.Delay((int)(maxDurationBeforeCharging * 1000));
        direction = (playerCoreSystem.transform.position - enemy.transform.position).normalized;
        await Task.Delay(300);
        isCharging = true;
        OnCharging();
    }
}
