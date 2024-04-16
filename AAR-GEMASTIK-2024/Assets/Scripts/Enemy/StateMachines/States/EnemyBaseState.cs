public abstract class EnemyBaseState
{
    private EnemyStateMachine enemyStateMachine;
    private EnemyBase enemy;

    public EnemyBaseState(EnemyStateMachine enemyStateMachine, EnemyBase enemy)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.enemy = enemy;
    }
    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract void OnUpdateState();
}
