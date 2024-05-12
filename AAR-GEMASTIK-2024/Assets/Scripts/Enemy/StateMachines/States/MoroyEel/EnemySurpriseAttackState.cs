using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySurpriseAttackState : EnemyBaseState
{
    private float forcePush = 30f;
    private EnemyBaseState nextState;
    private float radius = 4f;
    private bool withinBiteRange;
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    public EnemySurpriseAttackState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, float forcePush, float radius) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.forcePush = forcePush;
        this.radius = radius;
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        withinBiteRange = false;
        OnAttackAgain();
    }

    public override void OnExitState()
    {
        withinBiteRange = false;
    }

    public override void OnUpdateState()
    {
        if (playerCoreSystem.isDead) return;
        if (withinBiteRange) return;
        OnDetectingPlayer();
    }
    private void OnDetectingPlayer()
    {
        if(Vector3.Distance(enemy.headFish.transform.position, playerCoreSystem.transform.position) < radius)
        {
            withinBiteRange = true;
            Debug.Log("Player get caught");
            enemy.rigidBody.velocity = Vector3.zero;
            nextState.SetPlayerCoreSystem(playerCoreSystem);
            enemyStateMachine.OnTransitionState(nextState);
        }
    }
    private void OnAttackAgain()
    {
        Vector3 direction = (playerCoreSystem.transform.position - enemy.transform.position).normalized;
        enemy.rigidBody.AddForce(direction * forcePush, ForceMode.Impulse);
        AfterSurpriseAttack();
    }
    private async void AfterSurpriseAttack()
    {
        await Task.Delay(2000);
        if (withinBiteRange) return;
        enemy.rigidBody.velocity = Vector3.zero;
        Debug.Log(" Player didn't get caught");
    }

}
