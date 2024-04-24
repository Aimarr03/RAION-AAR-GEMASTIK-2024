using UnityEngine;
public class EnemyHammerhead : EnemyBase
{
    [Header("Idle State Data")]
    [SerializeField] private float detectionIdleRadius;
    [Header("Bite State Data")]
    [SerializeField] private float biteRadius;
    [SerializeField] private int biteDamage;
    [Header("Chase State Data")]
    [SerializeField] private float linearSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float maxLinearSpeed;

    private EnemyIdleState idleState;
    private EnemyBiteState biteState;
    private EnemyChaseState chaseState;
    public override void AddSuddenForce(Vector3 directiom, float forcePower)
    {
        
    }

    public override void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        
    }

    protected override void Awake()
    {
        base.Awake();
        healthSystem = new EnemyHealthSystem(this, health);
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask, detectionIdleRadius);
        biteState = new EnemyBiteState(stateMachine, this, playerLayerMask, biteRadius, biteDamage);
        chaseState = new EnemyChaseState(stateMachine, this, playerLayerMask, linearSpeed, rotatingSpeed, maxLinearSpeed);

        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        if(isDead) return;   
    }
}
