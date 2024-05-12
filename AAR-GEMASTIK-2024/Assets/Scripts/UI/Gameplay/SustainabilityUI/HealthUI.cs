using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerCoreSystem playerCoreSystem;
    [SerializeField] private Transform healthUI;
    [SerializeField] private Image healthBarImage;
    private void Awake()
    {
        healthUI = GetComponent<Transform>();
        if(playerCoreSystem == null)
        {
            playerCoreSystem = FindFirstObjectByType<PlayerCoreSystem>();
        }
    }
    private void Start()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Health).OnChangeValue += HealthUI_OnChangeValue;
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
        healthBarImage.fillAmount = 1;
    }
    private void OnDestroy()
    {
        playerCoreSystem.OnDead -= PlayerCoreSystem_OnDead;
    }
    private void PlayerCoreSystem_OnDead()
    {
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Health).OnChangeValue -= HealthUI_OnChangeValue;
    }

    private void HealthUI_OnChangeValue(SustainabilityData obj)
    {
        healthBarImage.fillAmount = obj.percentageValue;
    }
}
