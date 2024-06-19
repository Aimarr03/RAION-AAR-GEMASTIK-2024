using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkanKecilNetral : FishNeutralBase
{
    [SerializeField] private IdleCircleState idleCircleState;
    [SerializeField] private Panic02State panic02State;
    
    [Header("Idle Circle Data"), SerializeField] private float radiusDetection;
    [Range(0, 360), SerializeField] private float angle;
    [SerializeField] private float radiusDistance, speed;

    [Header("Panic 02 Data"), SerializeField] private float radiusPanicCheck;
    protected override void Awake()
    {
        base.Awake();
        idleCircleState = new IdleCircleState(this, stateMachine, playerMask, radiusDetection, angle, radiusDistance, speed);
        panic02State = new Panic02State(this, stateMachine, playerMask, speed *1.75f, radiusPanicCheck);
        idleCircleState.nextState = panic02State;
        panic02State.nextState = idleCircleState;
        stateMachine.InitializeState(idleCircleState);
    }

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
        
    }

    public override void Update()
    {
        base.Update();
    }

    

}
