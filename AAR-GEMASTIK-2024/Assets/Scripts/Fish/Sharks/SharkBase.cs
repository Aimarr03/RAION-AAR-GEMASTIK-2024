using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public static event Action OnEncounter;
    public static event Action OnNeutralized;
    public static event Action OnStopEncounter;
    public int bounty;
    private PlayerCoreSystem playerCoreSystem;
    protected bool isDisabled;

    [Header("Trigger Animator")]
    public string Die = "Dead";
    public string Surprise = "Surprise";
    public string Hit = "Hit";
    public string Happy = "Happy";
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
            isKnockout = true;
            isDisabled = true;
            rigidBody.velocity = Vector3.zero;
            transform.rotation =Quaternion.identity;
            StopAllCoroutines();
            OnNeutralized?.Invoke();
            animator.SetBool(Die, true);
            onTakeDamage -= SharkBase_onTakeDamage;

        }
    }

    public virtual void Update()
    {
        if (isKnockout)
        {
            if (isBeingHeld) OnBeingHeld();
            return;
        }
        if (isDisabled) return;
        if (isPause) return;
        stateMachine?.OnUpdate();
        
    }
    private void LateUpdate()
    {
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
        Debug.Log(health);
        Debug.Log(maxHealth);
        onTakeDamage?.Invoke(dead, ((float)health)/maxHealth);
        
    }

    public abstract void AddSuddenForce(Vector3 directiom, float forcePower);

    public virtual void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        OnDisabledDuration(moveDuration);
    }
    protected async void OnDisabledDuration(float moveDuration)
    {
        isDisabled = true;
        await Task.Delay((int)(moveDuration*1000));
        isDisabled = false;
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = true;
        if (playerCoreSystem == null) playerCoreSystem = playerInterractionSystem.getcore;
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
    public void OnInvokeEncounter() => OnEncounter?.Invoke();
    public void OnInvokeStopEncounter() => OnStopEncounter?.Invoke();
    public bool GetIsKnockout() => isKnockout;
}
