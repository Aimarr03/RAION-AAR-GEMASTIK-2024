using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkanKerapuNeutral : FishNeutralBase
{
    public Vector3 originalPosition;

    [SerializeField] private IdleCircleState idleCircleState;
    [SerializeField] private Panic02State panic02State;

    [SerializeField] private float radiusDetection;
    [SerializeField, Range(0,360)] private float angle;
    [SerializeField] private float radiusDistance;
    [SerializeField] private float speed;

    [SerializeField] private float radiusPanic;
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
    public override void Update()
    {
        base.Update();
    }

    protected override void Awake()
    {
        base.Awake();
        idleCircleState = new IdleCircleState(this, stateMachine, playerMask, radiusDetection, angle, radiusDistance, speed);
        panic02State = new Panic02State(this, stateMachine, playerMask, speed * 1.3f, radiusPanic);
        idleCircleState.nextState = panic02State;
        panic02State.nextState = idleCircleState;
        stateMachine.InitializeState(idleCircleState);
    }
}
