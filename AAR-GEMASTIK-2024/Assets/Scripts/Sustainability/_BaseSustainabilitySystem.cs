using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
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
public abstract class _BaseSustainabilitySystem
{
    protected PlayerCoreSystem player;
    protected float currentValue;
    protected float maxValue;
    public event Action<SustainabilityData> OnChangeValue;

    public _BaseSustainabilitySystem(PlayerCoreSystem player, float maxValue)
    {
        this.player = player;
        this.maxValue = maxValue;
        this.currentValue = maxValue;
    }
    public virtual void OnDecreaseValue(float value)
    {
        currentValue = Mathf.Clamp(currentValue - value, 0, maxValue);
        if (currentValue == 0)
        {
            player.SetDead();
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
    public SustainabilityData GetCurretnData(SustainabilityType type)
    {
        return new SustainabilityData(currentValue, maxValue, ChangeState.Increase, type);
    }
    //public abstract void OnUsage();
}
