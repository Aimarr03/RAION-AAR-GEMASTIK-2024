using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    [SerializeField] private PlayerCoreSystem playerCoreSystem;
    [SerializeField] private Transform oxygenUI;
    [SerializeField] private Image oxygenBarImage;
    private void Awake()
    {
        oxygenUI = GetComponent<Transform>();
    }
    private void Start()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Oxygen).OnChangeValue += OxygenUI_OnChangeValue;
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
        oxygenBarImage.fillAmount = 1;
    }
    private void OnDestroy()
    {
        playerCoreSystem.OnDead -= PlayerCoreSystem_OnDead;
    }
    private void PlayerCoreSystem_OnDead()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Oxygen).OnChangeValue -= OxygenUI_OnChangeValue;
    }

    private void OxygenUI_OnChangeValue(SustainabilityData obj)
    {
        oxygenBarImage.fillAmount = obj.percentageValue;
    }
}
