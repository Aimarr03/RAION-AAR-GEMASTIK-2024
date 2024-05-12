using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyFencingAttackState : EnemyBaseState
{
    private Transform centerPositionAttack;
    private Vector2 attackSize;
    private int attackDamage;
    private float forceMove;
    private EnemyBaseState nextState;
    public bool hasAttack;
    private bool onFencing;
    public EnemyFencingAttackState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask,
        Transform centerPositionAttack, Vector2 attackSize, int attackDamage, float forceMove) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.centerPositionAttack = centerPositionAttack;
        this.attackSize = attackSize;
        this.attackDamage = attackDamage;
        this.forceMove = forceMove;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerPositionAttack.position, attackSize);
    }

    public override void OnEnterState()
    {
        hasAttack = true;
        enemy.rigidBody.velocity = new Vector2(0.2f, 0);
        OnFencing();
    }

    public override void OnExitState()
    {
        hasAttack = false;
    }

    public override void OnUpdateState()
    {
        if (onFencing)
        {
            Collider[] playerAttacked = Physics.OverlapBox(centerPositionAttack.position, attackSize, Quaternion.identity, playerLayerMask);
            if(playerAttacked.Length > 0)
            {
                foreach(Collider player in playerAttacked)
                {
                    player.gameObject.TryGetComponent(out PlayerCoreSystem coreSystem);
                    coreSystem.TakeDamage(attackDamage);
                }
            }
        }
    }
    private async void OnFencing()
    {
        Vector3 direction = (playerCoreSystem.transform.position-enemy.transform.position).normalized;
        Debug.Log("On Hold");
        await Task.Delay(1800);
        Debug.Log("Attacking");
        enemy.rigidBody.AddForce(direction * forceMove, ForceMode.Impulse);
        onFencing = true;
        await Task.Delay(400);
        Debug.Log("Stop");
        onFencing = false;
        enemy.rigidBody.velocity = new Vector3(0.3f, 0);
        await Task.Delay(800);
        enemyStateMachine.OnTransitionState(nextState);
    }
}
