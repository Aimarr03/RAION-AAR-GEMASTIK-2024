using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponGameplayUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSystem playerWeaponSystem;

    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image backgroundIcon;

    private void Awake()
    {
        if(playerWeaponSystem == null) playerWeaponSystem = FindFirstObjectByType<PlayerCoreSystem>().weaponSystem;
        PlayerUsableGeneralData data = playerWeaponSystem.GetWeaponSO().generalData;
        //if(data.icon != null) weaponIcon.sprite = data.icon;
        backgroundIcon.sprite = weaponIcon.sprite;
        playerWeaponSystem.DoneFire += PlayerWeaponSystem_DoneFire;
    }
    private void OnDestroy()
    {
        playerWeaponSystem.DoneFire -= PlayerWeaponSystem_DoneFire;
    }

    private void PlayerWeaponSystem_DoneFire(float cooldown)
    {
        StartCoroutine(OnUICooldown(cooldown));
    }
    private IEnumerator OnUICooldown(float cooldown)
    {
        Debug.Log("Cooldown UI");
        float currentDuration = 0;
        weaponIcon.fillAmount = 0;
        while(currentDuration < cooldown)
        {
            currentDuration += Time.deltaTime;
            float percentage = (float)currentDuration / cooldown;
            //Debug.Log(percentage);
            weaponIcon.fillAmount = percentage;
            yield return null;
        }
        Debug.Log("Can be Used");
    }
}
