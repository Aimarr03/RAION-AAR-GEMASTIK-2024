using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumptionItemSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    [SerializeField] private ConsumableItemSO healthConsumptionSO;
    [SerializeField] private ConsumableItemSO oxygenConsumptionSO;
    [SerializeField] private ConsumableItemSO energyConsumptionSO;
    private Dictionary<SustainabilityType, ConsumableItemSO> ListConsumableSO;
    private int maxIndex = 2;
    private int currentIndex = 0;
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        ListConsumableSO = new Dictionary<SustainabilityType, ConsumableItemSO>
        {
            { SustainabilityType.Health, healthConsumptionSO },
            { SustainabilityType.Oxygen, oxygenConsumptionSO },
            { SustainabilityType.Energy, oxygenConsumptionSO}
        };

    }

    private SustainabilityType GetSustainabilityTypeBasedOnIndex()
    {
        switch (currentIndex)
        {
            case 0: return SustainabilityType.Health;
            case 1: return SustainabilityType.Oxygen;
            case 2: return SustainabilityType.Energy;
            default: return SustainabilityType.Capacity;
        }
    }
    public ConsumableItemSO GetConsumableItemSO(SustainabilityType type)
    {
        return ListConsumableSO[type];
    }
    public ConsumableItemSO GetConsumableItemSO()
    {
        return ListConsumableSO[GetSustainabilityTypeBasedOnIndex()];
    }
    private void onIncreaseIndex()
    {
        currentIndex++;
        if (currentIndex > maxIndex) currentIndex = 0;
    }
}
