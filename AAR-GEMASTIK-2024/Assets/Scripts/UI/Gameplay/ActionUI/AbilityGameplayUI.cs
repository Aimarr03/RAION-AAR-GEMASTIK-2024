using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityGameplayUI : MonoBehaviour
{
    [SerializeField] private PlayerAbilitySystem abilitySystem;

    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image backgroundIcon;
    private void Awake()
    {
        if (abilitySystem == null) abilitySystem = FindFirstObjectByType<PlayerCoreSystem>().abilitySystem;
        PlayerUsableGeneralData data = abilitySystem.GetAbilitySO().generalData;
        //if(data.icon != null ) abilityIcon.sprite = data.icon;
        backgroundIcon.sprite = abilityIcon.sprite;
        if(abilitySystem.GetAbilitySO().isInvokable) abilitySystem.OnDoneInvokingAbility += AbilitySystem_OnDoneInvokingAbility;
    }
    private void OnDestroy()
    {
        if (abilitySystem.GetAbilitySO().isInvokable) abilitySystem.OnDoneInvokingAbility -= AbilitySystem_OnDoneInvokingAbility;
    }

    private void AbilitySystem_OnDoneInvokingAbility(float cooldown)
    {
        StartCoroutine(OnUICooldown(cooldown));
    }
    private IEnumerator OnUICooldown(float cooldown)
    {
        Debug.Log("Cooldown Ability UI");
        float currentDuration = 0;
        abilityIcon.fillAmount = 0;
        while (currentDuration < cooldown)
        {
            currentDuration += Time.deltaTime;
            float percentage = (float)currentDuration / cooldown;
            //Debug.Log(percentage);
            abilityIcon.fillAmount = percentage;
            yield return null;
        }
        Debug.Log("Can be used");
    }
}
