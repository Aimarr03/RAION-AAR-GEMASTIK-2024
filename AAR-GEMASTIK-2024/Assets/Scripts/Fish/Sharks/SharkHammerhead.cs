using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkHammerhead : SharkBase
{
    [Header("Shark Idle State")]
    [SerializeField] private float radiusDetection;
    [SerializeField] private float idleAngle, x,y, idleSpeed;
    [Header("Shark Charge State")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private int chargeDamage;
    [SerializeField] private Transform attackCenter;
    [Header("Shark Chase State")]
    [SerializeField] private float linearSpeed;
    [SerializeField] private float rotatingSpeed, maxSpeed, distanceAggro;
    [SerializeField] private int chaseDamage;
    public SharkIdleBoxState idleBoxState;
    public SharkChargeState chargeState;
    public SharkChaseState chaseState;
    public override void AddSuddenForce(Vector3 directiom, float forcePower)
    {
        
    }

    public override void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void Awake()
    {
        base.Awake();
        idleBoxState = new SharkIdleBoxState(this, stateMachine, playerMask, radiusDetection, idleAngle, x, y, idleSpeed);
        chargeState = new SharkChargeState(this, stateMachine, playerMask, chargeSpeed, chargeDamage, attackCenter);
        chaseState = new SharkChaseState(this, stateMachine, playerMask, linearSpeed, rotatingSpeed, maxSpeed, distanceAggro, attackCenter, chaseDamage);
        chaseState.nextState = idleBoxState;
        idleBoxState.nextState = chargeState;
        chargeState.OnStopChargingState = idleBoxState;
        chargeState.OnAlternativeAttackState = chaseState;
        stateMachine.InitializeState(idleBoxState);
    }

    protected override void Start()
    {
        base.Start();
    }
}
