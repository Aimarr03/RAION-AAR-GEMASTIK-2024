using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
public enum ChangeState
{
    Increase,
    Decrease
}
public enum SustainabilityType
{
    Health,
    Energy,
    Oxygen,
    Capacity
}
public struct SustainabilityData
{
    public float currentValue;
    public float maxValue;
    public ChangeState changeState;
    public SustainabilityType type;
    public float percentageValue
    {
        get => currentValue / maxValue;
    }
    public bool isEmpty
    {
        get => currentValue <= 0;
    }
    public bool isFull
    {
        get => currentValue == maxValue;
    }
    public SustainabilityData(float currentHealth, float maxHealth, ChangeState changeState, SustainabilityType type)
    {
        this.currentValue = currentHealth;
        this.maxValue = maxHealth;
        this.changeState = changeState;
        this.type = type;
    }
}
public abstract class _BaseSustainabilitySystem : IDataPersistance
{
    protected PlayerCoreSystem player;
    protected float currentValue;
    protected float maxValue;
    protected SustainabilityType type;
    public event Action<SustainabilityData> OnChangeValue;
    string healthProblem = "Kapal Selam tidak kuat lagi bertahan dari serangan luar :<";
    string energyProblem = "Kapal Selam kehabisan energi untuk bergerak";
    string oxygenProblem = "Pengendara tidak bisa bernafas karena kehabisan oksigen di dalam kapal selam";
    public _BaseSustainabilitySystem(PlayerCoreSystem player, float maxValue, SustainabilityType type)
    {
        this.player = player;
        this.maxValue = maxValue;
        this.currentValue = maxValue;
        this.type = type;
    }
    public virtual void OnDecreaseValue(float value)
    {
        currentValue = Mathf.Clamp(currentValue - value, 0, maxValue);
        if (currentValue == 0)
        {
            string description = "";
            switch (type)
            {
                case SustainabilityType.Health:
                    description = healthProblem;
                    break;
                case SustainabilityType.Energy:
                    description = energyProblem;
                    break;
                case SustainabilityType.Oxygen:
                    description = oxygenProblem;
                    break;
            }
            player.SetDead(description);
        }
        else
        {
            SustainabilityData healthData = new SustainabilityData(currentValue, maxValue, ChangeState.Increase, SustainabilityType.Health);
            OnChangeValue?.Invoke(healthData);
        }
    }
    public virtual void OnIncreaseValue(float value)
    {
        currentValue = Mathf.Clamp(currentValue + value, 0, maxValue);
        SustainabilityData healthData = new SustainabilityData(currentValue, maxValue, ChangeState.Increase, SustainabilityType.Health);
        OnChangeValue?.Invoke(healthData);
        Debug.Log($"INcrease value by {value} => {currentValue}");
    }
    public virtual void OnAddMaxValue(int value) => maxValue += value;
    public SustainabilityData GetCurrentData(SustainabilityType type)
    {
        return new SustainabilityData(currentValue, maxValue, ChangeState.Increase, type);
    }
    public float GetCurrentValue() => currentValue;
    public void InvokeOnIncreaseValue(SustainabilityData data) => OnChangeValue?.Invoke(data);

    public void LoadScene(GameData gameData)
    {
        
    }

    public void SaveScene(ref GameData gameData)
    {
        
    }
    //public abstract void OnUsage();
}
