using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPorcupine : EnemyBase
{
    [SerializeField] private float radiusDetection;
    private EnemyIdleState idleState;
    private EnemyChaseState chaseState;
    private EnemySpikeState spikeState;
    protected override void Awake()
    {
        base.Awake();
        healthSystem = new EnemyHealthSystem(this, 250);
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask, radiusDetection);
        chaseState = new EnemyChaseState(stateMachine, this, playerLayerMask);
        spikeState =new EnemySpikeState(stateMachine, this, playerLayerMask);

        idleState.SetNextState(chaseState);
        chaseState.SetNextState(spikeState);
        spikeState.SetNextState(idleState);

        stateMachine.InitializeState(idleState);
    }
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
        
    }


    protected override void Update()
    {
        stateMachine.OnExecuteState();
    }

}
