using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlashAttackState : EnemyBaseState
{
    private Transform centerPositionAttack;
    private int damage;
    private float radius;
    private EnemyBaseState nextState;
    public bool hasAttack;

    public EnemySlashAttackState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, 
        Transform centerPositionAttack, int damage, float radius) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.centerPositionAttack = centerPositionAttack;
        this.damage = damage;
        this.radius = radius;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        hasAttack = true;
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        
    }

}
