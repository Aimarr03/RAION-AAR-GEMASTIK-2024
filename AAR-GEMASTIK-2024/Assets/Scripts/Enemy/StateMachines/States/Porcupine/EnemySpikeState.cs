using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySpikeState : EnemyBaseState
{
    private float attackRadius = 12f;
    private int damage;
    private EnemyBaseState nextState;
    private bool hasAttack;
    public EnemySpikeState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, int damage, float attackRadius) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.damage = damage;
        this.attackRadius = attackRadius;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        enemy.rigidBody.velocity = Vector3.zero;
        hasAttack = false;
    }

    public override void OnExitState()
    {
        hasAttack = false;
    }

    public override void OnUpdateState()
    {
        SpikeAttack();
    }
    private void SpikeAttack()
    {
        if (hasAttack) return;
        Collider[] UnitsGotHit = Physics.OverlapSphere(enemy.transform.position, attackRadius);
        if( UnitsGotHit.Length > 0)
        {
            foreach(Collider unit in UnitsGotHit)
            {
                if(unit.gameObject.TryGetComponent(out PlayerCoreSystem playerCoreSystem))
                {
                    IDamagable damagableUnit = playerCoreSystem;
                    damagableUnit.TakeDamage(damage);
                    Vector3 direction = (playerCoreSystem.transform.position - enemy.transform.position).normalized;
                    damagableUnit.AddSuddenForce(direction, 500f);
                    damagableUnit.OnDisableMove(2f, 25);
                }
            }
        }
        HasAttack();
    }
    private async void HasAttack()
    {
        hasAttack = true;
        await Task.Delay(8 * 1000);
        enemyStateMachine.OnTransitionState(nextState);
    }
}
