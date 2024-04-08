using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHarpoon : BaseWeapon
{
    public override void Fire(PlayerWeaponSystem weaponSystem)
    {
        if (isCooldown) return;
        Debug.Log("Harpoon Weapon is firing");
        Transform harpoonBullet = Instantiate(weaponSO.bullet, firePointBlank.position, Quaternion.identity);

        BaseBullet baseBullet = harpoonBullet.GetComponent<BaseBullet>();
        baseBullet.SetUpBullet(weaponSO.bulletData);

        StartCoroutine(ProcessCooldown());
    }

    public override IEnumerator ProcessCooldown()
    {
        Debug.Log("Is Cooldown");
        isCooldown = true;
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
