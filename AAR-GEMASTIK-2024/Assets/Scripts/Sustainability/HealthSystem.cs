using System;
using UnityEngine;

public class HealthSystem : _BaseSustainabilitySystem
{
    public HealthSystem(PlayerCoreSystem player, int maxValue, SustainabilityType type) : base(player, maxValue, type)
    {
        
    }
}
