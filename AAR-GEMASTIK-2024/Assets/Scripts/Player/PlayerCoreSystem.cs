using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreSystem : MonoBehaviour
{
    public PlayerMoveSystem moveSystem;
    public PlayerInputSystem inputSystem;
    public List<SustainabilitySystemSO> SustainabilitySystemsDataList;
    public Dictionary<SustainabilityType,_BaseSustainabilitySystem> _sustainabilitySystemsDictionary;
    public bool isDead;
    public event Action OnDead;
    private void Awake()
    {
        SetUpData();
        moveSystem = GetComponent<PlayerMoveSystem>();
        inputSystem = GetComponent<PlayerInputSystem>();
    }
    private void SetUpData()
    {
        _sustainabilitySystemsDictionary = new Dictionary<SustainabilityType, _BaseSustainabilitySystem>();
        foreach(SustainabilitySystemSO currentSustainabilityData in SustainabilitySystemsDataList)
        {
            SustainabilityType currentType = currentSustainabilityData.sustainabilityType;
            int maxValue = currentSustainabilityData.maxLevelTimesLevel;
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
    }
}

