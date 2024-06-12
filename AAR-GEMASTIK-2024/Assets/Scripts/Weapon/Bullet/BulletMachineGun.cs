using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMachineGun : BaseBullet
{
    public override void OnLaunchBullet()
    {
        transform.Translate(Time.deltaTime * weaponData.speed * transform.right);
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.TryGetComponent(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(weaponData.totalDamage);
                AudioManager.Instance.PlaySFX(OnHit);
            }

            LoadToPool();
            canLaunch = false;
        }
    }
}
