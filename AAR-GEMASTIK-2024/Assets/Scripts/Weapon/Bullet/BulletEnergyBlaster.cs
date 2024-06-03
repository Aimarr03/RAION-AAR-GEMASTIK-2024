using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnergyBlaster : BaseBullet
{
    [SerializeField] private float blastRadius;
    public override void OnLaunchBullet()
    {
        transform.position += Vector3.right * Time.deltaTime * weaponData.speed;
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }
    public void OnExplode()
    {
        Debug.Log("On Explode");
        Collider[] hitObjectWithinRadius = Physics.OverlapSphere(transform.position, blastRadius);
        for(int i = 0; i < hitObjectWithinRadius.Length; i++)
        {
            Collider currentHitWithinRadius = hitObjectWithinRadius[i];
            if (currentHitWithinRadius.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(weaponData.totalDamage);
            }
        }
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            if (weaponData.isFullyCharge)
            {
                OnExplode();
            }
            collision.gameObject.TryGetComponent(out IDamagable damagableUnit);
            damagableUnit.TakeDamage(weaponData.totalDamage);
            AudioManager.Instance.PlaySFX(OnHit);
            LoadToPool();
        }
    }
}
