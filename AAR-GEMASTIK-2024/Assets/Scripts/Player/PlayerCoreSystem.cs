using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerCoreSystem : MonoBehaviour, IDamagable
{
    public PlayerMoveSystem moveSystem;
    public PlayerInputSystem inputSystem;
    public PlayerWeaponSystem weaponSystem;
    public PlayerAbilitySystem abilitySystem;
    public PlayerConsumptionItemSystem consumptionItemSystem;
    public PlayerInterractionSystem interractionSystem;
    public Animator animator;
    public List<SustainabilitySystemSO> SustainabilitySystemsDataList;
    public bool isDead;
    public bool canBlock;
    public bool onDisabled;
    public event Action OnDead;
    public event Action OnBlocking;
    public event Action<bool> OnDisabled;
    public event Action OnBreakingFree;
    public RectTransform UIGuideDisable;

    private bool isVunerable;
    private float invunerableDuration;
    private float disabledDuration;
    private int attemptToRecover;
    private int maxAttempt;
    private bool isPaused;


    private Dictionary<SustainabilityType,_BaseSustainabilitySystem> _sustainabilitySystemsDictionary;
    
    [SerializeField] private float intervalUsageOxygen;
    private float currentDurationUsageOxygen;

    private void Awake()
    {
        SetUpData();
        moveSystem = GetComponent<PlayerMoveSystem>();
        inputSystem = GetComponent<PlayerInputSystem>();
        weaponSystem = GetComponent<PlayerWeaponSystem>();
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        interractionSystem = GetComponent<PlayerInterractionSystem>();
        consumptionItemSystem = GetComponent<PlayerConsumptionItemSystem>();

        currentDurationUsageOxygen = 0;
        isVunerable = true;
        onDisabled = false;
        invunerableDuration = 2f;
        disabledDuration = 0;
        maxAttempt = 12;
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.chosenWeaponSO != null) weaponSystem.SetWeaponSO(GameManager.Instance.chosenWeaponSO);
            if (GameManager.Instance.chosenAbilitySO != null) abilitySystem.SetUpAbilitySO(GameManager.Instance.chosenAbilitySO);
        }
        
    }
    private void Start()
    {
        ExpedictionManager.Instance.OnDoneExpediction += Instance_OnDoneExpediction;
        PauseGameplayUI.OnPause += PauseGameplayUI_OnPause;
    }

    private void PauseGameplayUI_OnPause(bool obj)
    {
        isPaused = obj;
        if (isPaused) OnDisableMove();
        else OnEnableMove();
    }

    private void OnDisable()
    {
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
        PauseGameplayUI.OnPause -= PauseGameplayUI_OnPause;
    }

    private void Instance_OnDoneExpediction(bool obj, PlayerCoreSystem coreSystem)
    {
        isPaused = true;
    }

    public void Update()
    {
        UIGuideDisable.parent.transform.rotation = Quaternion.identity;
        if (isDead) return;
        if(isPaused) return;
        OnUseOxygen();
        Test();
    }
    private void SetUpData()
    {
        _sustainabilitySystemsDictionary = new Dictionary<SustainabilityType, _BaseSustainabilitySystem>();
        foreach(SustainabilitySystemSO currentSustainabilityData in SustainabilitySystemsDataList)
        {
            SustainabilityType currentType = currentSustainabilityData.sustainabilityType;
            int maxValue = currentSustainabilityData.maxValueTimesLevel;
            Debug.Log($"{currentType} has max value of {maxValue}");
            _BaseSustainabilitySystem currentSustainabilitySystem = new HealthSystem(this, maxValue, SustainabilityType.Health);
            switch (currentType)
            {
                case SustainabilityType.Health:
                    currentSustainabilitySystem = new HealthSystem(this, maxValue, SustainabilityType.Health);
                    break;
                case SustainabilityType.Energy:
                    currentSustainabilitySystem = new EnergySystem(this, maxValue, SustainabilityType.Energy);
                    break;
                case SustainabilityType.Oxygen:
                    currentSustainabilitySystem = new OxygenSystem(this, maxValue, SustainabilityType.Oxygen);
                    break;
                case SustainabilityType.Capacity:
                    currentSustainabilitySystem = new WeightSystem(this, maxValue, SustainabilityType.Capacity);
                    break;
            }
            _sustainabilitySystemsDictionary.Add(currentType, currentSustainabilitySystem);
            Debug.Log($"Succesfully added {currentType} system into dictionary");
        }
    }
    public void SetDead(SustainabilityType type)
    {
        isDead = true;
        OnDead?.Invoke();
        GetComponent<Collider>().enabled = false;
        ExpedictionManager.Instance.InvokeOnLose(type);
        animator.SetBool("Dead", true);
        Debug.Log("Player Dead");
    }
    public void TakeDamage(int value)
    {
        if (!isVunerable) return;
        if (canBlock)
        {
            OnBlocking?.Invoke();
            return;
        }
        _sustainabilitySystemsDictionary[SustainabilityType.Health].OnDecreaseValue(value);
        Debug.Log("Player Take Damage with " + value + " quantity");
        InvunerabilityAfterTakeDamage();
    }
    public _BaseSustainabilitySystem GetSustainabilitySystem(SustainabilityType type)
    {
        return _sustainabilitySystemsDictionary[type];
    }
    private void OnUseOxygen()
    {
        currentDurationUsageOxygen += Time.deltaTime;
        if(currentDurationUsageOxygen >= intervalUsageOxygen)
        {
            currentDurationUsageOxygen = 0;
            OxygenSystem oxygenSystem = GetSustainabilitySystem(SustainabilityType.Oxygen) as OxygenSystem;
            oxygenSystem.OnDecreaseValue(1);
            Debug.Log("Oxygen System depleted by one");
        }
    }
    private async void InvunerabilityAfterTakeDamage()
    {
        animator.SetBool("Hurt", true);
        isVunerable = false;
        await Task.Delay((int)(invunerableDuration * 1000));
        animator.SetBool("Hurt", false);
        isVunerable = true;
    }
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10000);
        }
    }

    public void AddSuddenForce(Vector3 direction, float powerForce)
    {
        moveSystem.AddSuddenForce(direction, powerForce, ForceMode.Impulse); 
    }

    public void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        disabledDuration = moveDuration;
        maxAttempt = maxAttemptToRecover;
        DisableMovement(moveDuration);
    }
    public void OnDisableMove()
    {
        onDisabled = true;
        OnDisabled?.Invoke(onDisabled);
    }
    public void OnEnableMove()
    {
        onDisabled = false;
        OnDisabled?.Invoke(onDisabled);
    }
    private async void DisableMovement(float movementDuration)
    {
        UIGuideDisable.gameObject.SetActive(true);
        onDisabled = true;
        OnDisabled?.Invoke(onDisabled);
        moveSystem.SetCanBeUsed(false);
        attemptToRecover = 0;
        PlayerInputSystem.AttemptRecoverFromDisableStatus += PlayerInputSystem_AttemptRecoverFromDisableStatus;
        await Task.Delay((int)(movementDuration * 1000));
        if (!onDisabled) return;
        OnBreakingFree?.Invoke();
        onDisabled = false;
        OnDisabled?.Invoke(onDisabled);
        moveSystem.SetCanBeUsed(true);
        UIGuideDisable.gameObject.SetActive(false);
        PlayerInputSystem.AttemptRecoverFromDisableStatus -= PlayerInputSystem_AttemptRecoverFromDisableStatus;
    }

    private void PlayerInputSystem_AttemptRecoverFromDisableStatus()
    {
        if (isPaused) return;
        Debug.Log("Attempt To Recover");
        attemptToRecover++;
        if(attemptToRecover >= maxAttempt)
        {
            Debug.Log("attempt succesful");
            onDisabled = false;
            OnDisabled?.Invoke(onDisabled);
            moveSystem.SetCanBeUsed(true);
            PlayerInputSystem.AttemptRecoverFromDisableStatus -= PlayerInputSystem_AttemptRecoverFromDisableStatus;
            OnBreakingFree?.Invoke();
            UIGuideDisable.gameObject.SetActive(false);
        }
    }
    public void OnReplenishOxygen(int oxygen)
    {
        _BaseSustainabilitySystem oxygenSystem = GetSustainabilitySystem(SustainabilityType.Oxygen);
        oxygenSystem.OnIncreaseValue(oxygen);
    }
}

