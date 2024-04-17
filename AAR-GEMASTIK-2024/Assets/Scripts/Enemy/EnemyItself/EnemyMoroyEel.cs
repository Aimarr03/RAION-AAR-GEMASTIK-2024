using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoroyEel : EnemyBase
{
    [SerializeField] private float radius;
    private EnemyIdleState idleState;
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
        healthSystem = new EnemyHealthSystem(this, 200);

        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask, radius);

        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        stateMachine.OnExecuteState();
    }

}
