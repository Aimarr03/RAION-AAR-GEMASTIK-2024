using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletTorpedo : BaseBullet
{
    [SerializeField] private const string WALLTAG = "Wall";
    [SerializeField] private float radiusExplosion;
    [SerializeField] private Transform particleSystemOnExplode; 
    public override void OnLaunchBullet()
    {
        transform.position += Time.deltaTime * Vector3.right;
    }

    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();
    }
    
    private void OnExplode()
    {
        Transform particleSystemInstantiate = Instantiate(particleSystemOnExplode, transform.position, transform.rotation);
        AudioManager.Instance.PlaySFX(OnHit);
        foreach(ParticleSystem childParticleSystem in particleSystemInstantiate.GetComponentsInChildren<ParticleSystem>())
        {
            childParticleSystem.transform.parent = null;
            childParticleSystem.Play();
        }
        Destroy(particleSystemInstantiate.gameObject);
        Collider[] explodedUnit = Physics.OverlapSphere(transform.position, radiusExplosion);
        for(int index = 0; index < explodedUnit.Length; index++)
        {
            string gameObjectName = explodedUnit[index].gameObject.ToString();
            if (explodedUnit[index].gameObject.TryGetComponent(out IDamagable damagableUnit))
            {
                damagableUnit.TakeDamage(0);
            }
            Debug.Log($"{gameObjectName} is within the explosion area");
        }
        canLaunch = false;
        LoadToPool();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
        {
            damagableUnit.TakeDamage(0);
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
}
