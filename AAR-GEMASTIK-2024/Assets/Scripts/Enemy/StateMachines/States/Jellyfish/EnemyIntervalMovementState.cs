using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class EnemyIntervalMovementState : EnemyBaseState
{
    private EnemyBaseState nextState;
    private float radiusAggro = 5f;
    private float forcePush = 10f;
    private bool withinAggroArea = false;
    private bool canInvokeMovement = true;
    private Coroutine movementCoroutine;
    public EnemyIntervalMovementState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask) : base(enemyStateMachine, enemy, playerLayerMask)
    {

    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = withinAggroArea? Color.red : Color.green;
        Gizmos.DrawWireSphere(enemy.transform.position, radiusAggro);
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        OnIntervalMovement();
    }
    private void OnIntervalMovement()
    {
        if (!canInvokeMovement) return;
        enemy.rigidBody.velocity = Vector3.one;
        Vector3 direction = (playerCoreSystem.transform.position - enemy.transform.position).normalized;
        float distance = Vector3.Distance(enemy.transform.position, playerCoreSystem.transform.position);
        float forceCalculatePush = distance < 15 ? forcePush : forcePush * 2;
        enemy.rigidBody.AddForce(direction * forceCalculatePush, ForceMode.Impulse);
        CooldownInterval();
    }
    private async void CooldownInterval()
    {
        canInvokeMovement = false;
        Debug.Log("Cooldown");
        await Task.Delay(2 * 1000);
        float distance = Vector3.Distance(enemy.transform.position, playerCoreSystem.transform.position);
        if (distance < 2)
        {
            nextState.SetPlayerCoreSystem(playerCoreSystem);
            enemy.rigidBody.velocity = Vector3.zero;
            enemyStateMachine.OnTransitionState(nextState);
        }
        canInvokeMovement = true;
        Debug.Log("Move Again");
    }
   
    public void SetNextState(EnemyBaseState nextState)
    {
        this.nextState = nextState;
    }

    public override void OnEnterState()
    {
        
    }
}
