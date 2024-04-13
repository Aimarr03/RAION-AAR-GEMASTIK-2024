using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSystem : _BaseSustainabilitySystem
{
    public event Action UnableToAddWeight;
    public WeightSystem(PlayerCoreSystem player, int maxValue) : base(player, maxValue)
    {
        currentValue = 0;
    }

    public override void OnAddMaxValue(int value)
    {
        base.OnAddMaxValue(value);
    }

    public override void OnDecreaseValue(float value)
    {
        base.OnDecreaseValue(value);
    }

    public override void OnIncreaseValue(float value)
    {
        if (!canAddWeight(value))
        {
            UnableToAddWeight?.Invoke();
            return;
        }
        base.OnIncreaseValue(value);
        Debug.Log("Player remaining capacity " + (maxValue - currentValue));
    }
    public bool canAddWeight(float value)
    {
        bool condition = value + currentValue < maxValue;
        //Debug.Log("Condition on adding weight " + condition);
        return condition;
    }
}
