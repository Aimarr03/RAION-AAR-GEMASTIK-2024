public class EnemyStateMachine
{
    private EnemyBaseState currentState;
    private EnemyBase enemy;
    public EnemyStateMachine(EnemyBase enemy)
    {
        this.enemy = enemy;
    }
    public void OnExecuteState()
    {
        currentState?.OnUpdateState();
    }
    public void DrawGizmos()
    {
        currentState?.OnDrawGizmos();
    }
    public void OnTransitionState(EnemyBaseState newState)
    {
        currentState.OnExitState();
        currentState = newState;
        currentState.OnEnterState();
    }
    public void InitializeState(EnemyBaseState enemyBaseStateStarter)
    {
        currentState = enemyBaseStateStarter;
    }
    
}
