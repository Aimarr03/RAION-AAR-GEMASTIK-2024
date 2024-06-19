using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable, IInterractable, IDataPersistance
{
    [SerializeField] private string id;

    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    protected EnemyHealthSystem healthSystem;
    protected bool isKnockout = false;
    
    protected PlayerCoreSystem player;
    protected bool isBeingHeld;
    
    public Transform headFish;
    public RectTransform UI;
    
    protected EnemyStateMachine stateMachine;
    public Rigidbody rigidBody;
    public event Action OnHitEvent;
    [SerializeField] protected LayerMask playerLayerMask;

    public int damage;
    public int health;
    public string fishName;
    public float weight;
    public int bounty;

    public event Action OnEnemyDead;
    public static event Action<int> OnCaught;
    public static event Action OnEncounter;
    public void SetDead()
    {
        isKnockout = true;
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

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = true;
        playerInterractionSystem.SetIsHolding(true);
    }
    public void TriggerOnEncounter()
    {
        OnEncounter?.Invoke();
    }

    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = false;
        playerInterractionSystem.SetIsHolding(false);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out ExpedictionManager expedictionManager))
            {
                expedictionManager.OnGainCaughtFish(this);
            }
        }
    }
    public void OnHit()
    {
        OnHitEvent?.Invoke();
    }
    public void OnDelivered()
    {
        OnDeloading();
        OnCaught?.Invoke(bounty);
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (isKnockout)
        {
            player = coreSystem;
        }
    }
    protected void OnBeingHeld()
    {
        if (isBeingHeld && isKnockout && player != null)
        {
            transform.position = player.transform.position;
        }
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
    public bool GetIsFishKnockout() => isKnockout;
    
    private void OnDeloading()
    {
        GetComponent<SphereCollider>().enabled = false;
        int totalChild = transform.childCount;
        Debug.Log("Total Component: " + totalChild);
        for (int index = 0; index < totalChild; index++)
        {
            Transform currentChild = transform.GetChild(index);
            currentChild.gameObject.SetActive(false);
        }
    }
}
