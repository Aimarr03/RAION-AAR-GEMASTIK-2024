using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    protected EnemyHealthSystem healthSystem;
    protected bool isDead = false;
    public event Action OnEnemyDead;
    public void SetDead()
    {
        isDead = true;
        OnEnemyDead?.Invoke();
    }
    protected virtual void Awake()
    {
        
    }
    public abstract void AddSuddenForce(Vector3 directiom, float forcePower);

    public abstract void OnDisableMove(float moveDuration, int maxAttemptToRecover);

    public abstract void TakeDamage(int damage);
}
