using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float radius;
    private EnemyBaseState nextState;

    public EnemyIdleState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, float radius) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.radius = radius;
    }

    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    
    public override void OnEnterState()
    {
        
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        Collider[] collidedUnit = Physics.OverlapSphere(enemy.transform.position, radius, playerLayerMask);
        if (collidedUnit.Length > 0)
        {
            Collider player = collidedUnit[0];
            PlayerCoreSystem coreSystem = player.GetComponent<PlayerCoreSystem>();
            SetPlayerCoreSystem(coreSystem);
            nextState.SetPlayerCoreSystem(coreSystem);
            enemyStateMachine.OnTransitionState(nextState);
        }
    }
    public override void OnDrawGizmos()
    {
        Gizmos.color = playerCoreSystem != null ?  Color.red : Color.white;
        Gizmos.DrawWireSphere(enemy.transform.position, radius);
    }
}
