using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMachineGun : WeaponBase
{
    [SerializeField] private float durationToPrepareFire;
    private bool isFiring;
    private bool isOnRightDirection;
    private Coroutine ProcessFiring;
    private Coroutine OnFiring;
    private void OnEnable()
    {
        PlayerInputSystem.OnReleasedInvokeWeaponUsage += PlayerInputSystem_OnReleasedInvokeWeaponUsage;
    }

    private void PlayerInputSystem_OnReleasedInvokeWeaponUsage()
    {
        Debug.Log("Cancel Action");
        isFiring = false;
        StopAllCoroutines();
    }

    public override void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection)
    {
        this.isOnRightDirection = isOnRightDirection;
        if (!isFiring) ProcessFiring = StartCoroutine(ProcessCooldown());
        
    }
    private IEnumerator OnShootBullet()
    {
        float currentInterval = 0f;
        while (isFiring)
        {
            currentInterval += Time.deltaTime;
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
    }

    public override IEnumerator ProcessCooldown()
    {
        float currentTimer = 0f;
        while (currentTimer < durationToPrepareFire)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        isFiring = true;
        Debug.Log("FIRE!");
        OnFiring = StartCoroutine(OnShootBullet());
    }
}
