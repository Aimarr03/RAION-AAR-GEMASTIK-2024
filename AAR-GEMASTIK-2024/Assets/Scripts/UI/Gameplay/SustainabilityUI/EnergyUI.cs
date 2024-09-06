using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private PlayerCoreSystem playerCoreSystem;
    [SerializeField] private Transform energyUI;
    [SerializeField] private Image energyBarImage;
    private void Awake()
    {
        energyUI = GetComponent<Transform>();
        if(playerCoreSystem == null) playerCoreSystem = FindFirstObjectByType<PlayerCoreSystem>();
    }
    private void Start()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Energy).OnChangeValue += EnergyUI_OnChangeValue;
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
        energyBarImage.fillAmount = 1;
    }
    private void OnDestroy()
    {
        playerCoreSystem.OnDead -= PlayerCoreSystem_OnDead;
    }
    private void PlayerCoreSystem_OnDead()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Energy).OnChangeValue -= EnergyUI_OnChangeValue;
    }

    private void EnergyUI_OnChangeValue(SustainabilityData obj)
    {
        //Debug.Log(obj.ToString());
        energyBarImage.fillAmount = obj.percentageValue;
    }
}
