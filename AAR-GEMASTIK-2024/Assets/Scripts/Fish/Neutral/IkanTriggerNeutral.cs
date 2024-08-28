using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkanTriggerNeutral : FishNeutralBase
{
    public Vector3 originalPosition;

    public IdleHorizontallyState idleState;
    public PanicState panicState;
    [Range(0, 360f)]
    [SerializeField] private float angle;
    [SerializeField] private float radius;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private float radiusCheck;


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
        idleState = new IdleHorizontallyState(this, stateMachine, playerMask, maxDistance, radius, speed, angle);
        panicState = new PanicState(this, stateMachine, playerMask, speed * 1.8f, radiusCheck);

        idleState.nextState = panicState;
        panicState.nextState = idleState;

        stateMachine.InitializeState(idleState);
        originalPosition = transform.position;
    }
}
