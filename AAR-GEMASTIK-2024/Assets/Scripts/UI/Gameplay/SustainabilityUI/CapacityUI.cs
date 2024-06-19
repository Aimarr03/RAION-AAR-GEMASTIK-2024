using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapacityUI : MonoBehaviour
{
    [SerializeField] private PlayerCoreSystem playerCoreSystem;
    [SerializeField] private Transform capacityUI;
    [SerializeField] private TextMeshProUGUI capacityTextUI;
    private void Awake()
    {
        capacityUI = GetComponent<Transform>();
        if(playerCoreSystem == null) playerCoreSystem = FindFirstObjectByType<PlayerCoreSystem>();
    }
    private void Start()
    {
        _BaseSustainabilitySystem sustainabilitySystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity);
        sustainabilitySystem.OnChangeValue += CapacityUI_OnChangeValue;
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
        SustainabilityData sustainabilityData = sustainabilitySystem.GetCurrentData(SustainabilityType.Capacity);
        SetCapacityTextUI(sustainabilityData);
    }
    private void OnDestroy()
    {
        playerCoreSystem.OnDead -= PlayerCoreSystem_OnDead;
    }
    private void PlayerCoreSystem_OnDead()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnChangeValue -= CapacityUI_OnChangeValue;
    }

    private void CapacityUI_OnChangeValue(SustainabilityData data)
    {
        SetCapacityTextUI(data);
    }
    private void SetCapacityTextUI(SustainabilityData data)
    {
        float currentValue = data.currentValue;
        float maxValue = data.maxValue;
        capacityTextUI.text = $"{currentValue.ToString("0.00")} / {maxValue.ToString("0.00")}";
    }
}
