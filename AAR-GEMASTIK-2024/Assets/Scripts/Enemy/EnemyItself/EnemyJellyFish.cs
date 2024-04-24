using UnityEngine;

public class EnemyJellyFish : EnemyBase
{
    [Header("Idle State Data")]
    [SerializeField] private float idleRadius;
    [Header("Interval Move State Data")]
    [SerializeField] private float radiusAggro;
    [SerializeField] private float forcePush;
    [Header("Grab State Data")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int biteDamage;

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
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask, idleRadius);
        intervalMovementState = new EnemyIntervalMovementState(stateMachine, this, playerLayerMask, radiusAggro, forcePush);
        grabState = new EnemyGrabState(stateMachine, this, playerLayerMask, jumpForce, biteDamage);

        idleState.SetNextState(intervalMovementState);
        intervalMovementState.SetNextState(grabState);
        grabState.SetNextState(idleState);
        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        if (isDead) return;
        stateMachine.OnExecuteState();
    }
}
