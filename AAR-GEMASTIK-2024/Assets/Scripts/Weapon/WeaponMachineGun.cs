using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMachineGun : WeaponBase
{
    [SerializeField] private float durationToPrepareFire;
    private bool isFiring;
    private bool isOnRightDirection;
    [SerializeField] private float maxDuration;
    private void OnEnable()
    {
        PlayerInputSystem.OnReleasedInvokeWeaponUsage += PlayerInputSystem_OnReleasedInvokeWeaponUsage;
    }

    private void PlayerInputSystem_OnReleasedInvokeWeaponUsage()
    {
        Debug.Log("Cancel Action");
        isFiring = false;
    }

    public override void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection)
    {
        this.isOnRightDirection = isOnRightDirection;
        StartCoroutine(ProcessCooldown());
        
    }
    private IEnumerator OnShootBullet()
    {
        float totalDuration = 0f;
        float currentInterval = 0f;
        while (isFiring)
        {
            currentInterval += Time.deltaTime;
            totalDuration += Time.deltaTime;
            if (totalDuration > maxDuration)
            {
                isFiring = false;
                break;
            }
            if (currentInterval >= interval)
            {
                currentInterval = 0f;
                Debug.Log("FIRE BULLET");
                BaseBullet baseBullet = LoadBullet();
                Debug.Log(baseBullet.gameObject.name);
                baseBullet.SetUpBullet(weaponSO.bulletData, isOnRightDirection);
            }
            yield return null;
        }
        StartCoroutine(OnCooldown());
    }
    private IEnumerator OnCooldown()
    {
        Debug.Log("On Cooldown");
        isCooldown = true;
        yield return new WaitForSeconds(maxDuration * 2);
        Debug.Log("Can be used again");
        isCooldown = false;

    }

    public override IEnumerator ProcessCooldown()
    {
        if (isCooldown) yield break;
        float currentTimer = 0f;
        while (currentTimer < durationToPrepareFire)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        isFiring = true;
        Debug.Log("FIRE!");
        StartCoroutine(OnShootBullet());
    }
}
