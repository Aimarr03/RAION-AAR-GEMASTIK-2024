using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuntalAttackState : FishBaseState
{
    public FishBaseState nextState;
    private int damage;
    private float radiusTrigger;
    private bool onAttacking;
    public BuntalAttackState(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask, int damage, float radiusTrigger) : base(fish, fsm, playerMask)
    {
        this.damage = damage;
        this.radiusTrigger = radiusTrigger;
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        fish.animator.SetBool("Attack", true);
        fish.StartCoroutine(onStartAttacking());
    }

    public override void OnExitState()
    {
        onAttacking = false;
        fish.animator.SetBool("Attack", false);
    }

    public override void OnUpdateState()
    {
        if (onAttacking) OnTriggerAttacking();
    }
    private IEnumerator onStartAttacking()
    {
        float currentDuration = 0;
        float maxDuration = 5f;
        onAttacking = true;
        while(currentDuration < maxDuration)
        {
            currentDuration += Time.deltaTime;
            yield return null;
        }
        fsm.OnTransitionState(nextState);
    }
    private void OnTriggerAttacking()
    {
        Collider[] colliders = Physics.OverlapSphere(fish.transform.position, radiusTrigger, playerMask);
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }
    public void SetOnAttacking() => onAttacking = true;
}
