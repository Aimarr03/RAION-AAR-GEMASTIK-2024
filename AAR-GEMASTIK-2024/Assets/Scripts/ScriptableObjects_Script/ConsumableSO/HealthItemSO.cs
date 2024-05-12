using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Health Items", menuName = "Consumable Item/Create New Health Item SO")]
public class HealthItemSO : ConsumableItemSO
{
    public override SustainabilityType type => SustainabilityType.Health;
}
