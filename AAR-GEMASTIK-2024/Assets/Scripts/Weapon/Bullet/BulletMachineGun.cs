using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMachineGun : BaseBullet
{

    public override void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            Debug.Log("Hit " + collision.gameObject.name);
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
