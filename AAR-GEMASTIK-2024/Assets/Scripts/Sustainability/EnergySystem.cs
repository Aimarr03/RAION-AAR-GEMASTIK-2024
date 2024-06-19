using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : _BaseSustainabilitySystem
{
    public EnergySystem(PlayerCoreSystem player, int maxValue, SustainabilityType type) : base(player, maxValue, type)
    {
        player.moveSystem.OnUseOneEnergy += MoveSystem_OnUseOneEnergy;
        Debug.Log($"maxValue {maxValue}");
    }

    private void MoveSystem_OnUseOneEnergy()
    {
        OnDecreaseValue(1);
        Debug.Log($"remaining Energy = {currentValue}");
    }

}
