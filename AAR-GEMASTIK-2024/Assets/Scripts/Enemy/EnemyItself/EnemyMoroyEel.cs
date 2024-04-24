using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoroyEel : EnemyBase
{
    [SerializeField] private float radius;
    private EnemyHiddenState hiddenState;
    private EnemySurpriseAttackState surpriseAttackState;
    private EnemyInstantGrabState instantGrabState;
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
        healthSystem = new EnemyHealthSystem(this, 200);

        hiddenState = new EnemyHiddenState(stateMachine, this, playerLayerMask);
        surpriseAttackState = new EnemySurpriseAttackState(stateMachine, this, playerLayerMask);
        instantGrabState = new EnemyInstantGrabState(stateMachine, this, playerLayerMask);

        hiddenState.SetNextState(surpriseAttackState);
        surpriseAttackState.SetNextState(instantGrabState);
        instantGrabState.SetNextState(surpriseAttackState);

        stateMachine.InitializeState(hiddenState);
    }

    protected override void Update()
    {
        if (isDead) return;
        stateMachine.OnExecuteState();
    }

}
