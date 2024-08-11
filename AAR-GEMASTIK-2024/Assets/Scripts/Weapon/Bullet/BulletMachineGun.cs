using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMachineGun : BaseBullet
{
    public WeaponMachineGun machineGun => weaponBase as WeaponMachineGun;
    public int level => machineGun.level;
    [SerializeField] private float speed = 18f;
    public override void OnLaunchBullet()
    {
        transform.Translate(Time.deltaTime * transform.right * speed);
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.TryGetComponent(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(machineGun.GetMultiplierDamage(machineGun.level));
                AudioManager.Instance?.PlaySFX(OnHit);
            }

            LoadToPool();
            canLaunch = false;
        }
    }

    public override void SetUpBullet(bool isOnRightDirection, Quaternion angle)
    {
        base.SetUpBullet(isOnRightDirection, angle);
        speed = isOnRightDirection ? speed : -speed;
    }
}
