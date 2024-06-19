using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SharkBase : MonoBehaviour, IDamagable, IInterractable, IDelivarable, IDataPersistance
{
    [SerializeField] private string id;

    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public RectTransform UI;
    public LayerMask playerMask;
    [SerializeField] protected int maxHealth;
    protected int health;
    protected bool isKnockout;
    protected bool isDelievered;
    protected bool isBeingHeld;
    protected bool isPause;
    [SerializeField] public Rigidbody rigidBody;
    [SerializeField] public Collider sphereCollider;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Animator animator;
    [SerializeField] protected float weight;
    protected SharkStateMachine stateMachine;

    public event Action<bool, float> onTakeDamage;
    public static event Action OnNeutralized;
    public int bounty;
    private PlayerCoreSystem playerCoreSystem;
    protected virtual void Awake()
    {
        //rigidBody = GetComponent<Rigidbody>();
        //sphereCollider = GetComponent<SphereCollider>();
        stateMachine = new SharkStateMachine(this);
        health = maxHealth;
        isKnockout = false;
        isBeingHeld = false;
        isDelievered = false;
    }
    protected virtual void Start()
    {
        onTakeDamage += SharkBase_onTakeDamage;
        ExpedictionManager.Instance.OnLose += Instance_OnLose;
    }
    private void OnDisable()
    {
        ExpedictionManager.Instance.OnLose -= Instance_OnLose;
    }
    private void SharkBase_onTakeDamage(bool isDead, float percentage)
    {
        if (isDead)
        {
            this.isKnockout = true;
            OnNeutralized?.Invoke();
            animator.SetBool("Dead", true);
            onTakeDamage -= SharkBase_onTakeDamage;
        }
    }

    public virtual void Update()
    {
        if (isPause) return;
        if (isKnockout)
        {
            if (isBeingHeld) OnBeingHeld();
            return;
        }
        stateMachine?.OnUpdate();
        UI.rotation = Quaternion.identity;
    }
    public virtual void OnDrawGizmos()
    {
        stateMachine?.OnDraw();
    }
    public virtual void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        bool dead = health == 0;
        onTakeDamage?.Invoke(dead, health/maxHealth);
    }

    public abstract void AddSuddenForce(Vector3 directiom, float forcePower);

    public abstract void OnDisableMove(float moveDuration, int maxAttemptToRecover);

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = true;
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnIncreaseValue(weight);
        playerInterractionSystem.SetIsHolding(true);
    }

    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = false;
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnDecreaseValue(weight);
        playerInterractionSystem.SetIsHolding(false);
        playerCoreSystem = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent(out ExpedictionManager expedictionManager))
            {
                expedictionManager.OnGainCaughtFish(this);
                break;
            }
        }
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (!isKnockout) return;
        playerCoreSystem = coreSystem;
    }
    protected void OnBeingHeld()
    {
        transform.position = playerCoreSystem.transform.position;
    }
    public int GetBounty() => bounty;
    public DelivarableType GetDelivarableType() => DelivarableType.Mutated;
    public void OnDeloading()
    {
        isDelievered = true;
        GetComponent<Collider>().enabled = false;
        int totalChild = transform.childCount;
        Debug.Log("Total Component: " + totalChild);
        for (int index = 0; index < totalChild; index++)
        {
            Transform currentChild = transform.GetChild(index);
            currentChild.gameObject.SetActive(false);
        }
    }

    public void OnDelivered()
    {
        OnDeloading();
    }

    public void LoadScene(GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        levelData.sharkMutatedList.TryGetValue(id, out bool hasCollected);
        if (hasCollected)
        {
            OnDeloading();
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        if (levelData.sharkMutatedList.ContainsKey(id))
        {
            levelData.sharkMutatedList.Remove(id);
        }
        levelData.sharkMutatedList.Add(id, isKnockout);
    }
    private void Instance_OnLose(SustainabilityType obj)
    {
        isPause = true;
    }
    public bool HasBeenDelivered() => isDelievered;
}
