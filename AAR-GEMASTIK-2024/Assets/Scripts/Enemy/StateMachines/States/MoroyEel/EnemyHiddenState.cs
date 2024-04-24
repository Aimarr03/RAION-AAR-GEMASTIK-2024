using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHiddenState : EnemyBaseState
{
    private Vector3 boxSize = new Vector3(18, 16);
    private Vector3 GetPositionWithOffset
    {
        get => enemy.transform.position + (boxSize / 2);
    }
    private Vector3 GetPositionVerticalOffset
    {
        get
        {
            Vector3 offset = new Vector3(0, boxSize.y / 2, 0);
            return enemy.transform.position + offset;
        }
    }
    private EnemyBaseState nextState;
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;
    public EnemyHiddenState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, Vector2 boxSize) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.boxSize = boxSize;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = playerCoreSystem != null ? Color.red : Color.green;
        Gizmos.DrawWireCube(GetPositionVerticalOffset, boxSize);
    }

    public override void OnEnterState()
    {
        
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        Collider[] unitDetected = Physics.OverlapBox(GetPositionVerticalOffset, boxSize / 2, Quaternion.identity, playerLayerMask);
        if(unitDetected.Length > 0)
        {
            foreach(Collider collider in unitDetected)
            {
                if(collider.gameObject.TryGetComponent(out PlayerCoreSystem playerCoreSystem))
                {
                    if (playerCoreSystem == null) Debug.Log("Player Detected");
                    this.playerCoreSystem = playerCoreSystem;
                    nextState.SetPlayerCoreSystem(playerCoreSystem);
                    enemyStateMachine.OnTransitionState(nextState);
                }
            }
        }
        playerCoreSystem = null;
    }

}
