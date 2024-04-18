using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoroyEel : EnemyBase
{
    [SerializeField] private float radius;
    private EnemyHiddenState hiddenState;
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

        stateMachine.InitializeState(hiddenState);
    }

    protected override void Update()
    {
        stateMachine.OnExecuteState();
    }

}
