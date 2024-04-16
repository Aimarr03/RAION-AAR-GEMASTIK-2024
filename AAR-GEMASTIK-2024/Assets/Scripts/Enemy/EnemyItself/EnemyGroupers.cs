using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupers : EnemyBase
{
    [SerializeField] private float radius;
    private EnemyIdleState idleState;
    public override void AddSuddenForce(Vector3 directiom, float forcePower)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        stateMachine.DrawGizmos();
    }

    public override void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask,radius);
        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        stateMachine.OnExecuteState();
    }

    
}
