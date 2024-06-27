using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnergyBlaster : BaseBullet
{
    public WeaponEnergyBlaster energyBlaster => weaponBase as WeaponEnergyBlaster;
    public int level => energyBlaster.level;
    private bool isFullyCharge = false;
    private float speed = 20;
    [SerializeField] private float blastRadius;
    public override void OnLaunchBullet()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
        isFullyCharge = energyBlaster.GetIsFullyCharge();
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }
    public void OnExplode()
    {
        Debug.Log("On Explode");
        Collider[] hitObjectWithinRadius = Physics.OverlapSphere(transform.position, energyBlaster.GetMultiplierRadius(level));
        for(int i = 0; i < hitObjectWithinRadius.Length; i++)
        {
            Collider currentHitWithinRadius = hitObjectWithinRadius[i];
            if (currentHitWithinRadius.TryGetComponent(out PlayerCoreSystem coreSystem)) continue;
            if (currentHitWithinRadius.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(energyBlaster.GetMultiplierDamage(level));
            }
        }
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            if (isFullyCharge)
            {
                OnExplode();
            }
            collision.gameObject.TryGetComponent(out IDamagable damagableUnit);
            damagableUnit.TakeDamage(energyBlaster.GetMultiplierDamage(level));
            AudioManager.Instance?.PlaySFX(OnHit);
            LoadToPool();
        }
    }

    public override void SetUpBullet(bool isOnRightDirection, Quaternion angle)
    {
        base.SetUpBullet(isOnRightDirection, angle);
        speed = isOnRightDirection ? speed : -speed;
    }
}
