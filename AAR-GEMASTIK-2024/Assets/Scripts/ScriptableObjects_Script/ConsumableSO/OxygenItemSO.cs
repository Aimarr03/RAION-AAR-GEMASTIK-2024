using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Oxygen Items", menuName = "Consumable Item/Create New Oxygen Item SO")]
public class OxygenItemSO : ConsumableItemSO
{
    public override SustainabilityType type => SustainabilityType.Oxygen;
}
