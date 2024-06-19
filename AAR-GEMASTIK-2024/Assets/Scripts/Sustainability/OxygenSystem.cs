using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenSystem : _BaseSustainabilitySystem
{
    public OxygenSystem(PlayerCoreSystem player, int maxValue, SustainabilityType type) : base(player, maxValue, type)
    {
    }

}
