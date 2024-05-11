using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackChoiceHandlerForLongNoseShark : EnemyBaseState
{
    private EnemyFencingAttackState fencingState;
    private EnemySlashAttackState slashAttackState;
    private EnemyIdleState idleState;
    public EnemyAttackChoiceHandlerForLongNoseShark(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask,
        EnemyFencingAttackState fencingState, EnemySlashAttackState slashAttackState, EnemyIdleState idleState) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.fencingState = fencingState;
        this.slashAttackState = slashAttackState;
        this.idleState = idleState;
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        fencingState.SetPlayerCoreSystem(playerCoreSystem);
        enemyStateMachine.OnTransitionState(fencingState);
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        
    }

}
