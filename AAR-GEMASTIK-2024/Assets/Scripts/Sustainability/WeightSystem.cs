using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : _BaseSustainabilitySystem
{
    public static event Action<bool, float> OnOverweight;
    bool isOverweight = false;
    public WeightSystem(PlayerCoreSystem player, int maxValue, SustainabilityType type) : base(player, maxValue, type)
    {
        currentValue = 0;
        isOverweight = false;
    }

    public override void OnAddMaxValue(int value)
    {
        base.OnAddMaxValue(value);
    }

    public override void OnDecreaseValue(float value)
    {
        currentValue = Mathf.Clamp(currentValue - value, 0, maxValue);
        SustainabilityData healthData = new SustainabilityData(currentValue, maxValue, ChangeState.Increase, SustainabilityType.Health);
        InvokeOnIncreaseValue(healthData);
        Debug.Log("Current Value = " + currentValue);
        if (isOverweight)
        {
            if(currentValue <= maxValue)
            {
                isOverweight = false;
                OnOverweight?.Invoke(isOverweight, 1);
            }
        }
    }

    public override void OnIncreaseValue(float value)
    {
        currentValue = currentValue + value;
        SustainabilityData healthData = new SustainabilityData(currentValue, maxValue, ChangeState.Increase, SustainabilityType.Capacity);
        InvokeOnIncreaseValue(healthData);
        Debug.Log("Current Value = " + currentValue);
        isOverweight = currentValue > maxValue;
        if (isOverweight)
        {
            float percentage = maxValue / currentValue;
            OnOverweight?.Invoke(isOverweight, percentage);
        }
    }
    public bool canAddWeight(float value)
    {
        bool condition = value + currentValue < maxValue;
        //Debug.Log("Condition on adding weight " + condition);
        return condition;
    }
}
