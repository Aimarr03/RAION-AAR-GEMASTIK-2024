using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public WeaponBulletData weaponData;
    protected bool canLaunch = false;
    [SerializeField] protected float TimeToLive;
    public virtual void SetUpBullet(WeaponBulletData weaponData, bool isOnRightDirection)
    {
        weaponData.speed = isOnRightDirection ? weaponData.speed : -1 * weaponData.speed;
        this.weaponData = weaponData;
        Debug.Log("bullet data has been set up");
        canLaunch = true;
    }
    public abstract void Update();
    public abstract void OnCollisionEnter(Collision collision);
    public abstract void OnLaunchBullet();
}
