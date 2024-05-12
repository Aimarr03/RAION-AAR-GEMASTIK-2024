using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyThinkingChoiceState : EnemyBaseState
{
    private float durationMakingChoice = 1.3f;
    private float currentDuration = 0f;
    private bool needToThink;
    private float chaseStateDistance = 10f;
    private bool doneThinking; 

    public EnemyChaseState chaseState;
    public EnemyChargeState chargeState;
    public EnemyThinkingChoiceState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask) : base(enemyStateMachine, enemy, playerLayerMask)
    {

    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        doneThinking = false;
        currentDuration = 0f;
    }

    public override void OnExitState()
    {
        doneThinking = true;
        currentDuration = 0f;
    }

    public override void OnUpdateState()
    {
        if (doneThinking) return;
        if(currentDuration < durationMakingChoice)
        {
            currentDuration += Time.deltaTime;
        }
        else
        {
            doneThinking = true;
            currentDuration = 0f;
            if(Vector2.Distance(playerCoreSystem.transform.position, enemy.transform.position) < chaseStateDistance)
            {
                Debug.Log("Chase State!");
                chaseState.SetPlayerCoreSystem(playerCoreSystem);
                enemyStateMachine.OnTransitionState(chaseState);
                
            }
            else
            {
                Debug.Log("Charge State!");
                chargeState.SetPlayerCoreSystem(playerCoreSystem);
                enemyStateMachine.OnTransitionState(chargeState);
            }
        }
    }
}
