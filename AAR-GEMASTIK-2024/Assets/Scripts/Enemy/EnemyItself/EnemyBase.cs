using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    protected EnemyHealthSystem healthSystem;
    protected bool isDead = false;

    protected EnemyStateMachine stateMachine;
    [SerializeField] protected LayerMask playerLayerMask;

    public event Action OnEnemyDead;
    public void SetDead()
    {
        isDead = true;
        OnEnemyDead?.Invoke();
    }
    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine(this);
    }
    protected abstract void Update();
    public abstract void AddSuddenForce(Vector3 directiom, float forcePower);

    public abstract void OnDisableMove(float moveDuration, int maxAttemptToRecover);

    public abstract void TakeDamage(int damage);
    public abstract void OnDrawGizmos();
    
}
