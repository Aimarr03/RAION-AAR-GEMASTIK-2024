using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletTorpedo : BaseBullet
{
    public WeaponTorpedo weaponTorpedo => weaponBase as WeaponTorpedo;
    public int level => weaponTorpedo.level;
    [SerializeField] private const string WALLTAG = "Wall";
    [SerializeField] private float radiusExplosion;
    [SerializeField] private Transform particleSystemOnExplode;
    [SerializeField] private float speed = 25f;
    public override void OnLaunchBullet()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right, Space.Self);
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }
    
    private void OnExplode()
    {
        Transform particleSystemInstantiate = Instantiate(particleSystemOnExplode, transform.position, transform.rotation);
        AudioManager.Instance?.PlaySFX(OnHit);
        foreach(ParticleSystem childParticleSystem in particleSystemInstantiate.GetComponentsInChildren<ParticleSystem>())
        {
            childParticleSystem.transform.parent = null;
            childParticleSystem.Play();
        }
        Destroy(particleSystemInstantiate.gameObject);
        Collider[] explodedUnit = Physics.OverlapSphere(transform.position, weaponTorpedo.GetMultiplierRadius(level));
        for(int index = 0; index < explodedUnit.Length; index++)
        {
            string gameObjectName = explodedUnit[index].gameObject.ToString();
            if (explodedUnit[index].gameObject.TryGetComponent(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(weaponTorpedo.GetMultiplierDamage(level));
                damagableUnit.OnDisableMove(weaponTorpedo.GetMultiplierStunDuration(level), 10);
                damagableUnit.GetSlowed(weaponTorpedo.GetMultiplierSlowDuration(level), weaponTorpedo.GetMultiplierSlow(level));
            }
            Debug.Log($"{gameObjectName} is within the explosion area");
        }
        canLaunch = false;
        LoadToPool();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
        {
            damagableUnit.TakeDamage(weaponTorpedo.GetMultiplierDamage(level));
            damagableUnit.OnDisableMove(weaponTorpedo.GetMultiplierStunDuration(level), 10);
            damagableUnit.GetSlowed(weaponTorpedo.GetMultiplierSlowDuration(level), weaponTorpedo.GetMultiplierSlow(level));
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            damagableUnit.AddSuddenForce(direction, 12f);
            OnExplode();
        }
        if (collision.gameObject.CompareTag(WALLTAG))
        {
            OnExplode();
        }
        if (collision.gameObject.tag == "Testing")
        {
            OnExplode();
        }
    }

    public override void SetUpBullet(bool isOnRightDirection, Quaternion angle)
    {
        base.SetUpBullet(isOnRightDirection, angle);
        //speed = isOnRightDirection ? speed : -speed;
    }
}
