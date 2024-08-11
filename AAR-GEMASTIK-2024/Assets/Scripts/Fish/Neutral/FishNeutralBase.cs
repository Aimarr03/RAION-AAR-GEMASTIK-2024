using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class FishNeutralBase : MonoBehaviour, IDamagable
{
    public RectTransform UI;
    public LayerMask playerMask;
    [SerializeField] public Rigidbody2D rigidBody;
    [SerializeField] public Collider2D fishCollider;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Animator animator;
    protected FishNeutralStateMachine stateMachine;
    protected bool isPause;
    
    public static event Action OnNeutralized;
    protected virtual void Start()
    {
        ExpedictionManager.Instance.OnLose += Instance_OnLose;
    }
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        fishCollider = GetComponent<Collider2D>();
        stateMachine = new FishNeutralStateMachine(this);
        isPause = false;
    }
    public virtual void Update()
    {
        if (isPause) return;
        stateMachine?.OnUpdate();
        UI.rotation = Quaternion.identity;
    }
    public virtual void OnDrawGizmos()
    {
        stateMachine?.OnDraw();
    }
    public abstract void TakeDamage(int damage);

    public abstract void AddSuddenForce(Vector3 directiom, float forcePower);

    public abstract void OnDisableMove(float moveDuration, int maxAttemptToRecover);
    protected void Instance_OnLose(SustainabilityType obj)
    {
        isPause = true;
    }
}
