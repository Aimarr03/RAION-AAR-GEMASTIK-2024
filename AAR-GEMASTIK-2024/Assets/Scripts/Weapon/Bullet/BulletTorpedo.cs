using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletTorpedo : BaseBullet
{
    [SerializeField] private const string WALLTAG = "Wall";
    [SerializeField] private float radiusExplosion;
    public override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
        {
            OnExplode();
        }
        if (collision.gameObject.CompareTag(WALLTAG))
        {
            OnExplode();
        }
        if(collision.gameObject.tag == "Testing")
        {
            OnExplode();
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
    
    private void OnExplode()
    {
        Collider[] explodedUnit = Physics.OverlapSphere(transform.position, radiusExplosion);
        for(int index = 0; index < explodedUnit.Length; index++)
        {
            string gameObjectName = explodedUnit[index].gameObject.ToString();
            Debug.Log($"{gameObjectName} is within the explosion area");
        }
        Destroy(gameObject);
    }
}
