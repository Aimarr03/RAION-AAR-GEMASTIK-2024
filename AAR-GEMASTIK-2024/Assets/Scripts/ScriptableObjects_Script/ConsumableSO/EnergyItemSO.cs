using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Energy Items", menuName = "Consumable Item/Create New Energy Item SO")]
public class EnergyItemSO : ConsumableItemSO
{
    public override SustainabilityType type => SustainabilityType.Energy;
}
