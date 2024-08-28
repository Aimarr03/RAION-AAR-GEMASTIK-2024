using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkanSalmonNetral : FishNeutralBase
{
    public IdleBoxState idleBoxState;
    public Panic02State panic02State;
    [SerializeField] private float x, y;
    [SerializeField] private float radiusDetection;
    [Range(0,360)]
    [SerializeField] private float angle;
    [SerializeField] private float speed;
    [SerializeField] private float radiusCheck;
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
        base.OnDrawGizmos();
    }
    public override void Update()
    {
        base.Update();
    }

    protected override void Awake()
    {
        base.Awake();
        idleBoxState = new IdleBoxState(this, stateMachine, playerMask, radiusDetection, angle, x, y, speed);
        panic02State = new Panic02State(this, stateMachine, playerMask, speed * 1.33f, radiusCheck);

        idleBoxState.nextState = panic02State;
        panic02State.nextState = idleBoxState;
        stateMachine.InitializeState(idleBoxState);
    }
}
