using UnityEngine;

public class IkanBuntalNeutral : FishNeutralBase
{
    [SerializeField] private IdleCircleState idleCircleState;
    [SerializeField] private BuntalAttackState buntalAttackState;

    [SerializeField] private float radiusDetection;
    [SerializeField, Range(0, 360)] private float angle;
    [SerializeField] private float radiusDistance;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float radiusTrigger;
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
        buntalAttackState = new BuntalAttackState(this, stateMachine, playerMask, damage, radiusTrigger);
        idleCircleState.nextState = buntalAttackState;
        buntalAttackState.nextState = idleCircleState;
        stateMachine.InitializeState(idleCircleState);
    }
}
