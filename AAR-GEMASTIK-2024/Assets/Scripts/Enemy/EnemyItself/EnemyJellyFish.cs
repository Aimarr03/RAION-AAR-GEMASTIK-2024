using UnityEngine;

public class EnemyJellyFish : EnemyBase
{
    private EnemyIdleState idleState;
    private EnemyIntervalMovementState intervalMovementState;
    private EnemyGrabState grabState;
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
        healthSystem = new EnemyHealthSystem(this, 250);
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask,15);
        intervalMovementState = new EnemyIntervalMovementState(stateMachine, this, playerLayerMask);
        grabState = new EnemyGrabState(stateMachine, this, playerLayerMask);

        idleState.SetNextState(intervalMovementState);
        intervalMovementState.SetNextState(grabState);
        grabState.SetNextState(idleState);
        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        stateMachine.OnExecuteState();
    }
}