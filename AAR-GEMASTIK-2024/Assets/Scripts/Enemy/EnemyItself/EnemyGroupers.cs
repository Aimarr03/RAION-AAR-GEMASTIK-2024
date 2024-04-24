using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupers : EnemyBase
{
    [SerializeField] private float radius;
    private EnemyIdleState idleState;
    private EnemyChaseState chaseState;
    private EnemyBiteState biteState;
    public override void AddSuddenForce(Vector3 directiom, float forcePower)
    {
        
    }

    public override void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        
    }

    public override void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        stateMachine.DrawGizmos();
    }

    public override void TakeDamage(int damage)
    {
        healthSystem.OnDecreaseHealth(damage);
    }

    protected override void Awake()
    {
        base.Awake();
        healthSystem = new EnemyHealthSystem(this, 150);
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask,radius);
        chaseState = new EnemyChaseState(stateMachine, this, playerLayerMask);
        biteState = new EnemyBiteState(stateMachine, this, playerLayerMask);

        idleState.SetNextState(chaseState);
        chaseState.SetNextState(biteState);
        biteState.SetNextState(idleState);
        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        if (isDead) return;
        stateMachine.OnExecuteState();
    }

    
}
