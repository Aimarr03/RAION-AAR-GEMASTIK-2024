using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackChoiceHandlerForLongNoseShark : EnemyBaseState
{
    private EnemyFencingAttackState fencingState;
    private EnemySlashAttackState slashAttackState;
    private EnemyChaseState chasingState;
    public EnemyAttackChoiceHandlerForLongNoseShark(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask,
        EnemyFencingAttackState fencingState, EnemySlashAttackState slashAttackState, EnemyChaseState idleState) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.fencingState = fencingState;
        this.slashAttackState = slashAttackState;
        this.chasingState = idleState;
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        if (playerCoreSystem.isDead) return;
        if(Vector3.Distance(playerCoreSystem.transform.position, enemy.headFish.transform.position) > chasingState.GetAggroDistance())
        {
            Debug.Log("Chasing");
            chasingState.SetPlayerCoreSystem(playerCoreSystem);
            enemyStateMachine.OnTransitionState(chasingState);
            return;
        }
        int attackState = UnityEngine.Random.Range(0, 2);
        switch(attackState)
        {
            case 0:
                Debug.Log("Fencing");
                fencingState.SetPlayerCoreSystem(playerCoreSystem);
                enemyStateMachine.OnTransitionState(fencingState);
                break;
            case 1:
                Debug.Log("Slashing");
                slashAttackState.SetPlayerCoreSystem(playerCoreSystem);
                enemyStateMachine.OnTransitionState(slashAttackState);
                break;
        }
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        
    }

}
