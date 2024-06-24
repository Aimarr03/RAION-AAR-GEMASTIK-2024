using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMachineGun : BaseBullet
{
    public override void OnLaunchBullet()
    {
        transform.Translate(Time.deltaTime * transform.right);
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
                damagableUnit.TakeDamage(0);
                AudioManager.Instance.PlaySFX(OnHit);
            }

            LoadToPool();
            canLaunch = false;
        }
    }
}
