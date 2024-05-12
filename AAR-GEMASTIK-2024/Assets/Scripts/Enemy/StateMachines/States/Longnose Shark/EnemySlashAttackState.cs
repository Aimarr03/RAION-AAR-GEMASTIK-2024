using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySlashAttackState : EnemyBaseState
{
    private Transform centerPositionAttack;
    private int damage;
    private float radius;
    private EnemyBaseState nextState;
    public bool hasAttack;

    public EnemySlashAttackState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, 
        Transform centerPositionAttack, int damage, float radius) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.centerPositionAttack = centerPositionAttack;
        this.damage = damage;
        this.radius = radius;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPositionAttack.position, radius);
    }

    public override void OnEnterState()
    {
        hasAttack = false;
        OnAttacking();
    }

    public override void OnExitState()
    {
        hasAttack = false;
    }

    public override void OnUpdateState()
    {
        
    }
    private async void OnAttacking()
    {
        Debug.Log("Prepare to Slash");
        await Task.Delay(1600);
        Debug.Log("Slashing");
        hasAttack = true;
        Collider[] playerAttacked = Physics.OverlapSphere(centerPositionAttack.position, radius, playerLayerMask);
        if(playerAttacked.Length > 0)
        {
            foreach(Collider player in  playerAttacked)
            {
                player.TryGetComponent(out PlayerCoreSystem coreSystem);
                coreSystem.TakeDamage(damage);
            }
        }
        Debug.Log("cooling down");
        await Task.Delay(1800);
        enemyStateMachine.OnTransitionState(nextState);
    }

}
