using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyStateMachine enemyStateMachine;
    protected EnemyBase enemy;
    protected LayerMask playerLayerMask;
    protected PlayerCoreSystem playerCoreSystem;

    public EnemyBaseState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.enemy = enemy;
        this.playerLayerMask = playerLayerMask;
    }

    protected EnemyBaseState(EnemyStateMachine enemyStateMachine, EnemyBase enemy)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.enemy = enemy;
    }

    public void SetPlayerCoreSystem(PlayerCoreSystem playerCoreSystem) => this.playerCoreSystem = playerCoreSystem;
    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract void OnUpdateState();
    public abstract void OnDrawGizmos();
}
