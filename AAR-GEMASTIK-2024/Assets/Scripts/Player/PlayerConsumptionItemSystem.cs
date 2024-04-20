using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumptionItemSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    private ConsumableItemSO currentItemFocus;
    [SerializeField] private HealthItemSO healthConsumptionSO;
    [SerializeField] private OxygenItemSO oxygenConsumptionSO;
    [SerializeField] private EnergyItemSO energyConsumptionSO;
    private Dictionary<SustainabilityType, ConsumableItemSO> ListConsumableSO;
    private int maxIndex = 2;
    private int currentIndex = 0;
    [SerializeField] private float cooldownDuration;
    private bool isCooldown;
    private Coroutine cooldownIEnumerator;
    public event Action<SustainabilityType, int, float> onUseItem;
    public event Action<SustainabilityType, float> onChangeItem;
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        ListConsumableSO = new Dictionary<SustainabilityType, ConsumableItemSO>
        {
            { SustainabilityType.Health, healthConsumptionSO },
            { SustainabilityType.Oxygen, oxygenConsumptionSO },
            { SustainabilityType.Energy, energyConsumptionSO}
        };
        currentItemFocus = GetConsumableItemSO();
        isCooldown = false;
        Debug.Log(currentItemFocus.generalData.name);
    }
    private void Start()
    {
        PlayerInputSystem.InvokeSwitchItemFocus += PlayerInputSystem_InvokeSwitchItemFocus;
        PlayerInputSystem.InvokeUseItem += PlayerInputSystem_InvokeUseItem;
    }

    private void PlayerInputSystem_InvokeUseItem()
    {
        if (isCooldown) return;
        if (currentItemFocus.quantity <= 0)
        {
            onUseItem?.Invoke(currentItemFocus.type,currentItemFocus.quantity, 0);
            Debug.Log("Out of Item!");
            return;
        }
        Debug.Log("Use Item " + currentItemFocus.generalData.name);
        _BaseSustainabilitySystem sustainabilitySystem = coreSystem.GetSustainabilitySystem(currentItemFocus.type);
        sustainabilitySystem.OnIncreaseValue(currentItemFocus.GetTotalValueBasedOnTier());
        currentItemFocus.quantity--;
        if (cooldownIEnumerator != null) StopAllCoroutines();
        cooldownIEnumerator = StartCoroutine(OnCooldown(cooldownDuration));
        onUseItem?.Invoke(currentItemFocus.type, currentItemFocus.quantity, cooldownDuration);
    }
    private IEnumerator OnCooldown(float cooldownDuration)
    {
        isCooldown = true;
        float currentDuration = 0;
        while(currentDuration < cooldownDuration)
        {
            currentDuration += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        cooldownIEnumerator = null;
        Debug.Log("Item Can be Used Again");
    }
    private void PlayerInputSystem_InvokeSwitchItemFocus()
    {
        onIncreaseIndex();
        currentItemFocus = GetConsumableItemSO();
        float totalCooldownDuration = cooldownDuration / 4;
        if (cooldownIEnumerator != null)
        {
            StopAllCoroutines();
        }
        cooldownIEnumerator = StartCoroutine(OnCooldown(totalCooldownDuration));
        onChangeItem?.Invoke(currentItemFocus.type, totalCooldownDuration);
        Debug.Log(currentItemFocus.generalData.name);
    }

    public SustainabilityType GetSustainabilityTypeBasedOnIndex()
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
