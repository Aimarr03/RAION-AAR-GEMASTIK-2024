using System;
using System.Collections;
using System.Collections.Generic;
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
    public int currentValue;
    public int maxValue;
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
    public SustainabilityData(int currentHealth, int maxHealth, ChangeState changeState, SustainabilityType type)
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
    protected int currentValue;
    protected int maxValue;
    public Action<SustainabilityData> OnChangeValue;

    public _BaseSustainabilitySystem(PlayerCoreSystem player, int maxValue)
    {
        this.player = player;
        this.maxValue = maxValue;
        this.currentValue = maxValue;
    }
    public virtual void OnDecreaseValue(int value)
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
    public virtual void OnIncreaseValue(int value)
    {

    }
}