using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float radius;
    private EnemyBaseState nextState;

    public EnemyIdleState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask) : base(enemyStateMachine, enemy, playerLayerMask)
    {

    }

    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    
    public override void OnEnterState()
    {
        
    }

    public override void OnExitState()
    {
        Collider[] collidedUnit = Physics.OverlapSphere(enemy.transform.position, radius, playerLayerMask);
        if(collidedUnit.Length > 0 )
        {
            Collider player = collidedUnit[0];
            PlayerCoreSystem coreSystem = player.GetComponent<PlayerCoreSystem>();
            Debug.Log("Player Detected");
            SetPlayerCoreSystem(coreSystem);
        }
    }

    public override void OnUpdateState()
    {
        
    }
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(enemy.transform.position, radius);
    }
}
