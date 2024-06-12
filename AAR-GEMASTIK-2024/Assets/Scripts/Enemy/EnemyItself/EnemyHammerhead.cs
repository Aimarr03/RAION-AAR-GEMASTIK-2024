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
    [SerializeField] private float distanceAggro;
    [Header("Charge State Data")]
    [SerializeField] private float chargeSpeed;

    private EnemyIdleState idleState;
    private EnemyBiteState biteState;
    private EnemyChaseState chaseState;
    private EnemyThinkingChoiceState thinkingChoiceState;
    private EnemyChargeState chargeState;
    public override void AddSuddenForce(Vector3 directiom, float forcePower)
    {
        
    }

    public override void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        
    }

    public override void OnDrawGizmos()
    {
        if(stateMachine != null)
        {
            stateMachine.DrawGizmos();
        }
    }

    public override void TakeDamage(int damage)
    {
        healthSystem.OnDecreaseHealth(damage);
    }

    protected override void Awake()
    {
        base.Awake();
        healthSystem = new EnemyHealthSystem(this, health);
        idleState = new EnemyIdleState(stateMachine, this, playerLayerMask, detectionIdleRadius);
        biteState = new EnemyBiteState(stateMachine, this, playerLayerMask, biteRadius, biteDamage);
        chaseState = new EnemyChaseState(stateMachine, this, playerLayerMask, linearSpeed, rotatingSpeed, maxLinearSpeed, distanceAggro, headFish);
        thinkingChoiceState = new EnemyThinkingChoiceState(stateMachine, this, playerLayerMask);
        chargeState = new EnemyChargeState(stateMachine, this, playerLayerMask, chargeSpeed);

        idleState.SetNextState(thinkingChoiceState);
        chaseState.SetNextState(biteState);
        chargeState.SetNextState(thinkingChoiceState);

        thinkingChoiceState.chaseState = chaseState;
        thinkingChoiceState.chargeState = chargeState;
        stateMachine.InitializeState(idleState);
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
