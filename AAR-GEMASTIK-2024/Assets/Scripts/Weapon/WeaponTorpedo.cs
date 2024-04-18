using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTorpedo : WeaponBase
{
    public override void Fire(PlayerWeaponSystem coreSystem, bool isOnRightDirection)
    {
        if (isCooldown) return;
        Debug.Log("Torpedo Weapon is firing");
        
        BaseBullet baseBullet = LoadBullet();
        baseBullet.SetUpBullet(weaponSO.bulletData, isOnRightDirection);

        StartCoroutine(ProcessCooldown());
    }

    public override IEnumerator ProcessCooldown()
    {
        playerCoreSystem.weaponSystem.TriggerDoneFire(interval);
        Debug.Log("Is Cooldown");
        isCooldown = true;
        float currentInterval = 0;
        while (currentInterval <= interval)
        {
            currentInterval += Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
        Debug.Log("Cooldown done");
    }
}
