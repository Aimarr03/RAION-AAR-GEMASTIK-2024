using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreSystem : MonoBehaviour
{
    public PlayerMoveSystem moveSystem;
    public PlayerInputSystem inputSystem;
    public PlayerWeaponSystem weaponSystem;
    public List<SustainabilitySystemSO> SustainabilitySystemsDataList;
    public bool isDead;
    public event Action OnDead;
    private Dictionary<SustainabilityType,_BaseSustainabilitySystem> _sustainabilitySystemsDictionary;
    
    [SerializeField] private float intervalUsageOxygen;
    private float currentDurationUsageOxygen;

    private void Awake()
    {
        SetUpData();
        moveSystem = GetComponent<PlayerMoveSystem>();
        inputSystem = GetComponent<PlayerInputSystem>();
        weaponSystem = GetComponent<PlayerWeaponSystem>();
        currentDurationUsageOxygen = 0;
    }

    public void Update()
    {
        if (isDead) return;
        OnUseOxygen();
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
}

