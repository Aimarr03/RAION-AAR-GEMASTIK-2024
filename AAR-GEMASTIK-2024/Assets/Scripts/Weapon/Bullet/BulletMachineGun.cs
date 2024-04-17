using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMachineGun : BaseBullet
{

    public override void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.TryGetComponent(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(weaponData.totalDamage);
            }
            LoadToPool();
            canLaunch = false;
        }
    }

    public override void OnLaunchBullet()
    {
        transform.position += Time.deltaTime * weaponData.speed * Vector3.right;
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }
}
