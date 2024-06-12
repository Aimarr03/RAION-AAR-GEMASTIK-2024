using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHarpoon : WeaponBase
{
    public override void Fire(PlayerWeaponSystem weaponSystem, bool isOnRightDirection)
    {
        if (isCooldown) return;
        Debug.Log("Harpoon Weapon is firing");

        BaseBullet baseBullet = LoadBullet();
        baseBullet.SetUpBullet(weaponSO.bulletData, isOnRightDirection, playerCoreSystem.transform.rotation);
        StartCoroutine(ProcessCooldown());
    }

    public override IEnumerator ProcessCooldown()
    {
        Debug.Log("Is Cooldown");
        isCooldown = true;
        playerCoreSystem.weaponSystem.TriggerDoneFire(interval);
        float currentInterval = 0;
        while(currentInterval <= interval)
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done");
    }
}
