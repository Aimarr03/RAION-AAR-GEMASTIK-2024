using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    protected EnemyHealthSystem healthSystem;
    protected bool isDead = false;
    public Transform headFish;
    
    protected EnemyStateMachine stateMachine;
    public Rigidbody rigidBody;
    [SerializeField] protected LayerMask playerLayerMask;

    public int damage;
    public string fishName;

    public event Action OnEnemyDead;
    public void SetDead()
    {
        isDead = true;
        OnEnemyDead?.Invoke();
    }
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        stateMachine = new EnemyStateMachine(this);
    }
    public EnemyHealthSystem GetHealthSystem() => healthSystem;
    protected abstract void Update();
    public abstract void AddSuddenForce(Vector3 directiom, float forcePower);

    public abstract void OnDisableMove(float moveDuration, int maxAttemptToRecover);

    public abstract void TakeDamage(int damage);
    public abstract void OnDrawGizmos();
    
    
}
