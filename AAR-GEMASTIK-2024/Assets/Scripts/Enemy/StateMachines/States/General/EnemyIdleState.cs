using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float radius;
    private EnemyBaseState nextState;
    //private bool OnHitFirstTime;


    public EnemyIdleState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, float radius) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.radius = radius;
    }

    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    
    public override void OnEnterState()
    {
        enemy.rigidBody.velocity = Vector3.zero;
        enemy.OnHitEvent += Enemy_OnHitEvent;
        //OnHitFirstTime = false;
    }

    private void Enemy_OnHitEvent()
    {
        //OnHitFirstTime = true;
        Collider[] collidedUnit = Physics.OverlapSphere(enemy.transform.position, radius * 3, playerLayerMask);
        if (collidedUnit.Length > 0)
        {
            Collider player = collidedUnit[0];
            PlayerCoreSystem coreSystem = player.GetComponent<PlayerCoreSystem>();
            SetPlayerCoreSystem(coreSystem);
            nextState.SetPlayerCoreSystem(coreSystem);
            enemyStateMachine.OnTransitionState(nextState);
        }
        enemy.OnHitEvent -= Enemy_OnHitEvent;
    }

    public override void OnExitState()
    {
        enemy.TriggerOnEncounter();
        enemy.OnHitEvent -= Enemy_OnHitEvent;
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
