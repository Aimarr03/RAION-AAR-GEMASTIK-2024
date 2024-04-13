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
    public List<SustainabilitySystemSO> SustainabilitySystemsDataList;
    public bool isDead;
    public bool canBlock;
    public bool onDisabled;
    public event Action OnDead;
    public event Action OnBlocking;
    public event Action<bool> OnDisabled;

    private bool isVunerable;
    private float invunerableDuration;
    private float disabledDuration;
    private int attemptToRecover;
    private int maxAttempt;


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
        currentDurationUsageOxygen = 0;
        isVunerable = true;
        onDisabled = false;
        invunerableDuration = 2f;
        disabledDuration = 0;
        maxAttempt = 12;
    }

    public void Update()
    {
        if (isDead) return;
        OnUseOxygen();
        //Test();
    }
    private void SetUpData()
    {
        _sustainabilitySystemsDictionary = new Dictionary<SustainabilityType, _BaseSustainabilitySystem>();
        foreach(SustainabilitySystemSO currentSustainabilityData in SustainabilitySystemsDataList)
        {
            SustainabilityType currentType = currentSustainabilityData.sustainabilityType;
            int maxValue = currentSustainabilityData.maxLevelTimesLevel;
            Debug.Log($"{currentType} has max value of {maxValue}");
            _BaseSustainabilitySystem currentSustainabilitySystem = new HealthSystem(this, maxValue);
            switch (currentType)
            {
                case SustainabilityType.Health:
                    currentSustainabilitySystem = new HealthSystem(this, maxValue);
                    break;
                case SustainabilityType.Energy:
                    currentSustainabilitySystem = new EnergySystem(this, maxValue);
                    break;
                case SustainabilityType.Oxygen:
                    currentSustainabilitySystem = new OxygenSystem(this, maxValue);
                    break;
                case SustainabilityType.Capacity:
                    currentSustainabilitySystem = new WeightSystem(this, maxValue);
                    break;
            }
            _sustainabilitySystemsDictionary.Add(currentType, currentSustainabilitySystem);
            Debug.Log($"Succesfully added {currentType} system into dictionary");
        }
    }
    public void SetDead()
    {
        isDead = true;
        OnDead?.Invoke();
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
        isVunerable = false;
        await Task.Delay((int)(invunerableDuration * 1000));
        isVunerable = true;
    }
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void AddSuddenForce(Vector3 direction, float powerForce)
    {
        moveSystem.AddSuddenForce(direction, powerForce, ForceMode.Force); 
    }

    public void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {
        disabledDuration = moveDuration;
        maxAttempt = maxAttemptToRecover;
        DisableMovement(moveDuration);
    }
    private async void DisableMovement(float movementDuration)
    {
        onDisabled = true;
        OnDisabled?.Invoke(onDisabled);
        moveSystem.SetCanBeUsed(false);
        attemptToRecover = 0;
        PlayerInputSystem.AttemptRecoverFromDisableStatus += PlayerInputSystem_AttemptRecoverFromDisableStatus;
        await Task.Delay((int)(movementDuration * 1000));
        if (!onDisabled) return;
        onDisabled = false;
        OnDisabled?.Invoke(onDisabled);
        moveSystem.SetCanBeUsed(true);
        PlayerInputSystem.AttemptRecoverFromDisableStatus -= PlayerInputSystem_AttemptRecoverFromDisableStatus;
    }

    private void PlayerInputSystem_AttemptRecoverFromDisableStatus()
    {
        Debug.Log("Attempt To Recover");
        attemptToRecover++;
        if(attemptToRecover >= maxAttempt)
        {
            Debug.Log("attempt succesful");
            onDisabled = false;
            OnDisabled?.Invoke(onDisabled);
            moveSystem.SetCanBeUsed(true);
            PlayerInputSystem.AttemptRecoverFromDisableStatus -= PlayerInputSystem_AttemptRecoverFromDisableStatus;
        }
    }
}

