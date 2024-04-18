using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBiteState : EnemyBaseState
{
    private float radiusBite = 3;
    private bool hasBitten = false;
    private EnemyBaseState nextState;
    public EnemyBiteState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask) : base(enemyStateMachine, enemy, playerLayerMask)
    {

    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        Debug.Log("Going for a bite");
        hasBitten = false;
    }

    public override void OnExitState()
    {
        hasBitten = false;
    }

    public override void OnUpdateState()
    {
        OnBiteAttempt();
    }
    private void OnBiteAttempt()
    {
        if (hasBitten) return;
        Collider[] detectedUnit = Physics.OverlapSphere(enemy.headFish.transform.position, radiusBite, playerLayerMask);
        if(detectedUnit.Length > 0)
        {
            foreach(Collider collider in detectedUnit)
            {
                if(collider.gameObject.TryGetComponent(out PlayerCoreSystem playerCoreSystem))
                {
                    playerCoreSystem.TakeDamage(enemy.damage);
                    break;
                }
            }
        }
        TakeBreak();
    }
    private async void TakeBreak()
    {
        hasBitten = true;
        Debug.Log("take break");
        await Task.Delay(4 * 1000);
        Debug.Log("Set Iddling");
        enemyStateMachine.OnTransitionState(nextState);
    }

}
