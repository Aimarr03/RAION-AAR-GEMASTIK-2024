using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPorcupine : EnemyBase
{
    [Header("Idle Data")]
    [SerializeField] private float radiusDetection;
    [Header("Move Speed Data")]
    [SerializeField] private float linearSpeed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float maxLinearSpeed;
    [SerializeField] private float distanceAggro;
    [Header("Spike Data")]
    [SerializeField] private float radiusAttack;
    [SerializeField] private int attackDamage;

    private EnemyIdleState idleState;
    private EnemyChaseState chaseState;
    private EnemySpikeState spikeState;
    protected override void Awake()
    {
        base.Awake();
        healthSystem = new EnemyHealthSystem(this, health);
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask, radiusDetection);
        chaseState = new EnemyChaseState(stateMachine, this, playerLayerMask, linearSpeed, angularSpeed, maxLinearSpeed, distanceAggro, transform);
        spikeState =new EnemySpikeState(stateMachine, this, playerLayerMask, attackDamage, radiusAttack);

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
        healthSystem.OnDecreaseHealth(damage);
    }


    protected override void Update()
    {
        if (isKnockout)
        {
            OnBeingHeld();
            return;
        }
        stateMachine.OnExecuteState();
    }

}
