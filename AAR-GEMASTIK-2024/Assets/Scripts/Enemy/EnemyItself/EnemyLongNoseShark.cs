using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLongNoseShark : EnemyBase
{
    [Header("Idle State")]
    [SerializeField] private float radiusIdleState;

    [Header("Chasing State")]
    [SerializeField] private float linearSpeed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float maxLinearSpeed;
    [SerializeField] private float distanceAggro;

    [Header("Fencing State")]
    [SerializeField] private Transform fencingCenterPositionAttack;
    [SerializeField] private Vector2 fencingAttackSize;
    [SerializeField] private int fencingAttackDamage;
    [SerializeField] private float fencingForceMove;

    [Header("Slashing State")]
    [SerializeField] private Transform slashingCenterPositionAttack;
    [SerializeField] private int slashingDamage;
    [SerializeField] private float slashingRadius;

    private EnemyIdleState idleState;
    private EnemyChaseState chaseState;
    
    private EnemyFencingAttackState fencingAttackState;
    private EnemySlashAttackState slashAttackState;
    private EnemyAttackChoiceHandlerForLongNoseShark attackHandlerState;
    

    protected override void Awake()
    {
        base.Awake();
        healthSystem = new(this, health);
        idleState = new(stateMachine, this, playerLayerMask, radiusIdleState);
        chaseState = new(stateMachine, this, playerLayerMask, linearSpeed, angularSpeed, maxLinearSpeed, distanceAggro, headFish);
        fencingAttackState = new(stateMachine, this, playerLayerMask, headFish, fencingAttackSize, fencingAttackDamage, fencingForceMove);
        slashAttackState = new(stateMachine, this, playerLayerMask, headFish, slashingDamage, slashingRadius);
        attackHandlerState = new(stateMachine, this, playerLayerMask, fencingAttackState, slashAttackState, chaseState);

        idleState.SetNextState(chaseState);
        chaseState.SetNextState(attackHandlerState);
        slashAttackState.SetNextState(attackHandlerState);
        fencingAttackState.SetNextState(attackHandlerState);

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
