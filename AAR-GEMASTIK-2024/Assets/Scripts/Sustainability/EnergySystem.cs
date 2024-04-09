using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : _BaseSustainabilitySystem
{
    public EnergySystem(PlayerCoreSystem player, int maxValue) : base(player, maxValue)
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
