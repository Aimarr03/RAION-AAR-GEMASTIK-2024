using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public WeaponBulletData weaponData;

    public void SetUpBullet(WeaponBulletData weaponData)
    {
        this.weaponData = weaponData;
        Debug.Log("bullet data has been set up");
    }
    public abstract void Update();
    public abstract void OnCollisionEnter(Collision collision);
}
